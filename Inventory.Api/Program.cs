using Inventory.Infra.Database;
using Microsoft.EntityFrameworkCore;
using Inventory.App;
using Inventory.Domain.Repositories;
using Inventory.Infra.Repositories;
using Autofac;
using Inventory.Api.AutofacModules;
using Inventory.App.IntegrationEvents;
using Inventory.AppIntegrationEvents;
using System.Reflection;
using Oths.EventBus.Abstractions;
using MediatR;
using Serilog;
using Oths.EventBus;
using Oths.EventBus.RabbitMQ;
using RabbitMQ.Client;
using Inventory.Api.Controllers;
using Autofac.Extensions.DependencyInjection;
using Inventory.App.Queries;

var configuration = GetConfiguration();

Log.Logger = CreateSerilogLogger(configuration);

Log.Information("Configuring web host ({ApplicationContext})...", Program.AppName);

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new MediatorModule()));

builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new ApplicationModule(configuration["ConnectionString"])));

// Add services to the container.

ConfigureServices(builder.Services);

AddCustomIntegrations(builder.Services, configuration);

//-------------- logging ---------------------
//added: Configure JSON logging to the console.
builder.Logging.AddJsonConsole();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<ItemsDbContext>(opt =>
{
    opt.UseInMemoryDatabase("itemsdb")/*, ServiceLifetime.Singleton*/;
    opt.EnableSensitiveDataLogging(true) ;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

ConfigureMockData(app);

app.Run();



/////////
IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}

void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();

    services.AddEndpointsApiExplorer();

    services.AddSwaggerGen();

    //custom
    services.AddMediatR(Assembly.GetExecutingAssembly());

    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
    {
        var subscriptionClientName = builder.Configuration["SubscriptionClientName"];
        var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
        var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
        var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
        var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

        var retryCount = 5;
        if (!string.IsNullOrEmpty(builder.Configuration["EventBusRetryCount"]))
        {
            retryCount = int.Parse(builder.Configuration["EventBusRetryCount"]);
        }

        return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
    });

    services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
}

Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
{
    var seqServerUrl = configuration["Serilog:SeqServerUrl"];
    var logstashUrl = configuration["Serilog:LogstashgUrl"];
    return new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .Enrich.WithProperty("ApplicationContext", Program.AppName)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://logstash:8080" : logstashUrl, null)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

void AddCustomIntegrations(IServiceCollection services, IConfiguration configuration)
{
    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

    services.AddTransient<IInventoryIntegrationEventService, InventoryIntegrationEventService>();

    services.AddScoped<IItemTypesService, ItemTypesService>();

    services.AddScoped<IItemQueries, ItemQueries>();

    services.AddScoped(typeof(IEFRepository<>), typeof(EFRepository<>));

    services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
    {
        var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

        var factory = new ConnectionFactory()
        {
            HostName = configuration["EventBusConnection"],
            DispatchConsumersAsync = true
        };

        if (!string.IsNullOrEmpty(configuration["EventBusPort"]))
        {
            var isValid = int.TryParse(configuration["EventBusPort"], out var port);
            if (isValid)
            {
                factory.Port = port;
            }
        }

        if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
        {
            factory.UserName = configuration["EventBusUserName"];
        }

        if (!string.IsNullOrEmpty(configuration["EventBusPassword"]))
        {
            factory.Password = configuration["EventBusPassword"];
        }

        var retryCount = 5;
        if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
        {
            retryCount = int.Parse(configuration["EventBusRetryCount"]);
        }

        return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
    });

}

void ConfigureMockData(IApplicationBuilder app)
{
    using (var scope = app.ApplicationServices.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ItemsDbContext>();

            context.AddTestData();
        }
        catch (Exception ex)
        {
        }
    }
}

public partial class Program
{
    public static string Namespace = typeof(ItemsController).Namespace;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}


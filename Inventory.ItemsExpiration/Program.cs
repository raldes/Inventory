using Autofac.Extensions.DependencyInjection;
using Inventory.ItemsExpiration;
using Inventory.ItemsExpiration.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

public class Program
{
    public static readonly string AppName = typeof(Program).Assembly.GetName().Name;

    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Run();
    }

    public static IHost CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
            .ConfigureAppConfiguration((host, builder) =>
            {
                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile("appsettings.json", optional: true);
                builder.AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", optional: true);
                builder.AddEnvironmentVariables();
                builder.AddCommandLine(args);
            })
            .ConfigureLogging((host, builder) => builder.UseSerilog(host.Configuration).AddSerilog())
            .Build();
}


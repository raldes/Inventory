using Microsoft.EntityFrameworkCore;
using Inventory.App;
using Inventory.Domain.Repositories;
using Inventory.Infra.Database;
using Inventory.Infra.Repositories;
using Inventory.App.Queries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//-------------- logging ---------------------
//added: Configure JSON logging to the console.
builder.Logging.AddJsonConsole();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<ItemsDbContext>(opt =>
{
    opt.UseInMemoryDatabase("itemsdb")/*, ServiceLifetime.Singleton*/;
    opt.EnableSensitiveDataLogging(true);
});

builder.Services.AddScoped<IItemTypesService, ItemTypesService>();

builder.Services.AddScoped<IItemsService, ItemsService>();

builder.Services.AddScoped<IItemQueries, ItemQueries>();

builder.Services.AddScoped(typeof(IEFRepository<>), typeof(EFRepository<>));
///


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Items}/{action=Index}/{id?}");

ConfigureMockData(app);

app.Run();

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


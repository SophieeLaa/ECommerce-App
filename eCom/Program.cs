using eCom.Data;
using eCom.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IActorsService, ActorsService>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(GetConnectionString(builder.Configuration)), ServiceLifetime.Scoped);
string GetConnectionString(IConfiguration configuration)
{
    return configuration.GetConnectionString("DefaultConnectionString");
}

//Action<SqlServerDbContextOptionsBuilder>? GetConnectionString(string v)
//{
//    ///throw new NotImplementedException();
//    return configuration.GetConnectionString("DefaultConnectionString");
//}


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        // Apply migrations
        var dbContext = services.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();

        // Seed data
        AppDbInitializer.Seed(app);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during database initialization: {ex.Message}");
    }

}

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
    pattern: "{controller=Movies}/{action=Index}/{id?}");

app.Run();

using App.Logger;
using App.Services;
using AspNetCore.SEOHelper;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Omega_Store.Services;
using Store.Data;
using System.Configuration;

var config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json")
             .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connectionString;
var env = builder.Environment;

try
{
    if (!env.IsDevelopment())
    {
        connectionString = config["ConnectionString"];
    }
    else
    {
        connectionString = config["ConnectionStringDev"];
    }
}
catch (Exception)
{
    connectionString = "";
}

builder.Services.AddDbContext<Store.Data.DbContext>(
             o => o.UseSqlServer(connectionString)
              );

var roota = AppDomain.CurrentDomain;
var root = roota.BaseDirectory;
string folder = Path.Combine(root, "Files");
//string sqlitePath = Path.Combine(folder, "omegaDBMainTest.db");
//string sqliteConnect = $"Data Source={sqlitePath}";

string sqlitePath2 = Path.Combine(folder, "dataLogger.db");
string sqliteConnect2 = $"Data Source={sqlitePath2}";

List<string> readData = new List<string>();
if (!(Directory.Exists(folder)))
{
    Directory.CreateDirectory(folder);
}
else
{
}

//builder.Services.AddDbContext<Store.Data.DbContext>(
//  o => o.UseSqlite(sqliteConnect)
//   );
builder.Services.AddDbContext<LogContext>(
  o => o.UseSqlite(sqliteConnect2)
   );
builder.Services.AddHttpClient();
builder.Services.AddSignalR();
builder.Services.AddDirectoryBrowser();
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = long.MaxValue;
});

DependecyInjection.Register(builder.Services);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

try
{
    using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
    {
        scope.ServiceProvider.GetService<Store.Data.DbContext>().Database.Migrate();
    }
}
catch (Exception e)
{
    FileService.WriteToFile("\n\n" + e, "ErrorLogs");
}

try
{
    using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
    {
        scope.ServiceProvider.GetService<LogContext>().Database.Migrate();
    }
}
catch (Exception e)
{
    FileService.WriteToFile("\n\n" + e, "ErrorLogs");
}

app.UseHttpsRedirection();


if (!env.IsDevelopment())
{
    try
    {
        HttpService.BaseUrl = "https://trendycampus.com/";
        app.UseStaticFiles(new StaticFileOptions
        {

            FileProvider = new PhysicalFileProvider(
                       Path.Combine(env.ContentRootPath, "Images")),
            RequestPath = "/trig"
        });

        app.UseStaticFiles(new StaticFileOptions
        {

            FileProvider = new PhysicalFileProvider(
            Path.Combine(env.ContentRootPath, "Videos")),
            RequestPath = "/vids"
        });

        app.UseStaticFiles(new StaticFileOptions
        {

            FileProvider = new PhysicalFileProvider(
             Path.Combine(env.ContentRootPath, "Documents")),
            RequestPath = "/docs"
        });
    }
    catch (Exception e)
    {
        FileService.WriteToFile("\n\n" + e, "ErrorLogs");
    }

}
else
{
    HttpService.BaseUrl = "https://localhost:7161/";
    try
    {
        app.UseStaticFiles(new StaticFileOptions
        {

            FileProvider = new PhysicalFileProvider(
                       "C:\\Users\\USER\\Documents\\Projects\\2024\\Web\\TSVE Foods\\Omega Store\\bin\\Debug\\net8.0\\Images"),
            RequestPath = "/trig"
        });

        app.UseStaticFiles(new StaticFileOptions
        {

            FileProvider = new PhysicalFileProvider(
            "C:\\Users\\USER\\Documents\\Projects\\2024\\Web\\TSVE Foods\\Omega Store\\bin\\Debug\\net8.0\\Videos"),
            RequestPath = "/vids"
        });

        app.UseStaticFiles(new StaticFileOptions
        {

            FileProvider = new PhysicalFileProvider(
             "C:\\Users\\USER\\Documents\\Projects\\2024\\Web\\TSVE Foods\\Omega Store\\bin\\Debug\\net8.0\\Documents"),
            RequestPath = "/docs"
        });
    }
    catch (Exception e)
    {
        FileService.WriteToFile("\n\n" + e, "ErrorLogs");
    }
}



app.UseStaticFiles();
app.UseXMLSitemap(app.Environment.ContentRootPath);

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapHub<DefaultHub>("/meeting");

app.Run();

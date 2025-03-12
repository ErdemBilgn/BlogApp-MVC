using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BlogContext>(options =>
{
    ConfigurationManager config = builder.Configuration;
    string? connectionString = config.GetConnectionString("mysql_connection");
    // options.UseSqlite(connectionString);
    var version = new MySqlServerVersion(new Version(8, 4, 4));
    options.UseMySql(connectionString, version);
});

builder.Services.AddScoped<IPostRepository, EfPostRepository>();
builder.Services.AddScoped<ITagRepository, EfTagRepository>();

var app = builder.Build();

app.UseStaticFiles();

SeedData.SeedTestDatas(app);

app.MapDefaultControllerRoute();

app.Run();

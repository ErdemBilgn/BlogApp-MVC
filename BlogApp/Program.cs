using BlogApp.Data.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<BlogContext>(options =>
{
    ConfigurationManager config = builder.Configuration;
    string? connectionString = config.GetConnectionString("mysql_connection");
    // options.UseSqlite(connectionString);
    var version = new MySqlServerVersion(new Version(8, 4, 4));
    options.UseMySql(connectionString, version);
});

var app = builder.Build();

SeedData.SeedTestDatas(app);

app.MapGet("/", () => "Hello World!");

app.Run();

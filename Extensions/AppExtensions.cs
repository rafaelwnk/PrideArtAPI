using Microsoft.EntityFrameworkCore;
using PrideArtAPI.Data;

namespace PrideArtAPI.Extensions;

public static class AppExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(connectionString));
    }
}
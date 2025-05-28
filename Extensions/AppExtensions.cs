using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PrideArtAPI.Data;
using PrideArtAPI.Services;

namespace PrideArtAPI.Extensions;

public static class AppExtensions
{

    public static void LoadConfiguration(this WebApplicationBuilder builder)
    {
        Configuration.JwtKey = builder.Configuration.GetValue<string>("JwtKey")!;
    }

    public static void ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
    }

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(connectionString));

        builder.Services.AddTransient<TokenService>();
    }
}
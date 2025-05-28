using PrideArtAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.LoadConfiguration();
builder.ConfigureAuthentication();
builder.ConfigureServices();

var app = builder.Build();
app.Run();



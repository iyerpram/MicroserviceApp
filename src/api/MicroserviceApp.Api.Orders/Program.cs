using MicroserviceApp.Logic.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddApiServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.EnableApiServices();

app.Run();

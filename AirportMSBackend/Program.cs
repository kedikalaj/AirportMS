using Microsoft.Extensions.DependencyInjection;
using Sentry.Extensions.Logging.Extensions.DependencyInjection;
using Sentry.Profiling;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.AddSentry(options =>
{
    options.Dsn = "https://ee3949679b9394adfbd6ec713e7ae710@o4507302947717120.ingest.de.sentry.io/4507329946124368";
    // Additional configuration options can be set here
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(builder =>
{
    builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin();
});


app.Run();


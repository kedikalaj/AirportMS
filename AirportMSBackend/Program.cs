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
builder.Logging.AddSentry(o =>
{
    o.Dsn = "https://ee3949679b9394adfbd6ec713e7ae710@o4507302947717120.ingest.de.sentry.io/4507329946124368";
    o.Debug = true;
    // Set TracesSampleRate to 1.0 to capture 100%
    // of transactions for performance monitoring.
    // We recommend adjusting this value in production
    o.TracesSampleRate = 1.0;
    // Sample rate for profiling, applied on top of othe TracesSampleRate,
    // e.g. 0.2 means we want to profile 20 % of the captured transactions.
    // We recommend adjusting this value in production.
    o.ProfilesSampleRate = 1.0;
    // Requires NuGet package: Sentry.Profiling
    // Note: By default, the profiler is initialized asynchronously. This can
    // be tuned by passing a desired initialization timeout to the constructor.
    o.AddIntegration(new ProfilingIntegration(
        // During startup, wait up to 500ms to profile the app startup code.
        // This could make launching the app a bit slower so comment it out if you
        // prefer profiling to start asynchronously.
        TimeSpan.FromMilliseconds(500)));
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


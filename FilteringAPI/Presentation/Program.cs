using Application.BackgroundServices;
using Application.Services.Abstractions;
using Application.Services.Concrete;
using Domain.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddControllers();

builder.Services.Configure<FilteringOptions>(builder.Configuration.GetSection("Filtering"));

builder.Services.AddScoped<IChunkService, ChunkService>();

builder.Services.AddSingleton<IQueueService, QueueService>();
builder.Services.AddSingleton<IUploadBufferService, UploadBufferService>();
builder.Services.AddSingleton<IResultService, ResultService>();
builder.Services.AddSingleton<IFilteringService, FilteringService>();

builder.Services.AddHostedService<FilteringBackgroundService>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen((options) =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Filtering API",
        Version = "v1",
        Description = "API documentation"
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => { c.DisplayRequestDuration(); });

app.MapControllers();

var asciiArt = File.ReadAllText("ascii-art.txt");
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine(asciiArt);
Console.ResetColor();

app.Run();
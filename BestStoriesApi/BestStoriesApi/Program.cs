using BestStoriesApi.BackgroundServices;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddHostedService<StoriesWorker>();

string redisConnection = Environment.GetEnvironmentVariable("RedisConnection", EnvironmentVariableTarget.Process);

builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = redisConnection; });

builder.Services.AddControllers();



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();

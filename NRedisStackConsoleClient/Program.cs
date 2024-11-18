using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using NRedisStack.RedisStackCommands;

// Connect to Redis
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();


var connectionString = configuration.GetConnectionString("Redis");
var redis = await ConnectionMultiplexer.ConnectAsync(connectionString);
var db = redis.GetDatabase();

// Access stack commands using NRedisStack
var jsonCommands = db.JSON();

// Example of using JSON.SET command to save an object
var jsonData = new { Name = "Vitaliy", Age = 52, City = "Sochi" };
await jsonCommands.SetAsync("user:NumberTwo", "$", jsonData);

// Example of using JSON.GET command to retrieve an object
var result = await jsonCommands.GetAsync("user:NumberTwo");
Console.WriteLine($"Retrieved data: {result}");

// Save changes before closing
await db.ExecuteAsync("SAVE");

// Close the connection
await redis.CloseAsync();
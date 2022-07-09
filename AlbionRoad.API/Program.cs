using AlbionRoad.Resources.Configs;
using AlbionRoad.Application.Handlers;
using AlbionRoad.Domain.Interfaces.Services;
using AlbionRoad.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var albionData = builder.Configuration.GetSection(AlbionDataSettings.SECTION);
builder.Services.Configure<AlbionDataSettings>(albionData);

var redisSection = builder.Configuration.GetSection(RedisSettings.SECTION);
builder.Services.Configure<RedisSettings>(redisSection);

var redisSettings = redisSection.Get<RedisSettings>();

// Redis
builder.Services.AddStackExchangeRedisCache(opts =>
{
    opts.Configuration = $"localhost:6379";
});

builder.Services.AddDistributedMemoryCache();

// DI
builder.Services.AddScoped<TravelHandler>();
builder.Services.AddScoped<IItemService, ItemService>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
builder.Services.AddHttpClient();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();

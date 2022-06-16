using AlbionRoad.Resources.Configs;
using AlbionRoad.Application.Handlers;
using AlbionRoad.Domain.Interfaces.Services;
using AlbionRoad.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var albionData = builder.Configuration.GetSection("AlbionData");
builder.Services.Configure<AlbionData>(albionData);

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

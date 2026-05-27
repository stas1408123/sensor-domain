using AutoMapper.Extensions.ExpressionMapping;
using Sensor.API.GraphQL.Query;
using Sensor.API.GraphQL.Types;
using Sensor.BLL.DI;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});
// Add services to the container.

builder.Services.AddControllers();
builder.Services
    .AddGraphQLServer()
    .AddQueryType<RoomQuery>()
    .AddType<RoomType>()
    .AddType<AirQualityType>()
    .AddType<EnergyType>()
    .AddType<MotionType>();


builder.Services.AddTransient<RoomQuery>();

builder.Services.AddBusinessLogicDependency(config);
builder.Services.AddAutoMapper(x =>
{
    x.AddExpressionMapping();
},
typeof(Sensor.BLL.Mappers.MappingProfile).Assembly, typeof(Sensor.API.Mappers.MappingProfile).Assembly);
var app = builder.Build();
app.Logger.LogInformation("Sensor API is starting");

// Configure the HTTP request pipeline.

if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapGraphQL();


app.MapControllers();

app.Run();

public partial class Program
{
}

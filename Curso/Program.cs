using Microsoft.EntityFrameworkCore;
using Curso.Context;
using Serilog;
using Curso.Producer;
using Microsoft.Extensions.Configuration;
using System.Runtime;

var builder = WebApplication.CreateBuilder(args);

//Agrego el servicio de Log.
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();
builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Kafka
//Host.CreateDefaultBuilder(args)
//    .ConfigureAppConfiguration((hostingContext, config) =>
//    {
//        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
//    })
//    .ConfigureServices((hostContext, services) =>
//    {
//        services.Configure<KafkaOptions>(hostContext.Configuration.GetSection("Kafka"));
//        services.AddControllers();
//    });

builder.Services.AddScoped<IProducer, KafkaProducer>();

var app = builder.Build();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

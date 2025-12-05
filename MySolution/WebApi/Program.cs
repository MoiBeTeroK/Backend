
using Dapper;
using FluentValidation;
using WebApi.Conf;
using WebApi.BLL.Services;
using System.Text.Json;
using WebApi.Jobs;
// создается билдер веб приложения
var builder = WebApplication.CreateBuilder(args);
DefaultTypeMap.MatchNamesWithUnderscores = true;
builder.Services.AddScoped<UnitOfWork>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Information); 

builder.Services.Configure<DbSettings>(builder.Configuration.GetSection(nameof(DbSettings)));
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection(nameof(RabbitMqSettings)));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IAuditLogOrderRepository, AuditOrderRepository>();

builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<AuditLogService>();
builder.Services.AddScoped<RabbitMqService>();

builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));
builder.Services.AddScoped<ValidatorFactory>();
// зависимость, которая автоматически подхватывает все контроллеры в проекте
builder.Services.AddControllers().AddJsonOptions(options => 
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
});
// добавляем swagger
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<OrderGenerator>();

// собираем билдер в приложение
var app = builder.Build();

// добавляем 2 миддлвари для обработки запросов в сваггер
app.UseSwagger();
app.UseSwaggerUI();

// добавляем миддлварю для роутинга в нужный контроллер
app.MapControllers();

// вместо *** должен быть путь к проекту Migrations
// по сути в этот момент будет происходить накатка миграций на базу
Migrations.Program.Main([]); 

// запускам приложение
app.Run();
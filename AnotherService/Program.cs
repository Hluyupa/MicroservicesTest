using AnotherService.Consumers;
using BrokersModels;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(p =>
{
    // p.AddConsumer<RabbitMqConsumer>();
    //
    // p.UsingRabbitMq((context, cfg) =>
    // {
    //     cfg.Host("localhost", "/", c =>
    //     {
    //         c.Username("guest");
    //         c.Password("guest");
    //     });
    //     cfg.ConfigureEndpoints(context);
    // });
    
    // Добавляем базовый InMemory транспорт для корректной работы MassTransit
    const string topicName = "my-topic";
    const string consumerGroup = "first-consumer-group";
    const string kafkaBrokerServer = "localhost:9092";
    
    p.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));
    
    // Конфигурация для Kafka
    p.AddRider(rider =>
    {
        rider.AddConsumer<KafkaMessageConsumer>(); // Добавляем консюмера для Kafka
        rider.UsingKafka((context, k) =>
        {
            k.Host(kafkaBrokerServer);
            k.TopicEndpoint<KafkaMessage>(topicName, consumerGroup, e =>
            {
                e.ConfigureConsumer<KafkaMessageConsumer>(context);
            });
        });
    });
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "KafkaMessageIdHolder";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();

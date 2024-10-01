using BrokersModels;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddMassTransit(p =>
{
    // p.UsingRabbitMq((context, cfg) =>
    // {
    //     cfg.Host("localhost", "/", c =>
    //     {
    //         c.Username("guest");
    //         c.Password("guest");
    //     });
    //     cfg.ConfigureEndpoints(context);
    // });
    
    p.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
    p.AddRider(rider =>
    {
        const string topicName = "my-topic";
        const string kafkaBrokerServer = "localhost:9092";
    
        rider.AddProducer<KafkaMessage>(topicName);
        rider.UsingKafka((context, k) =>
        {
            k.Host(kafkaBrokerServer);
        });
    });
});


var app = builder.Build();

app.UseRouting();
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();

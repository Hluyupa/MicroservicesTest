using BrokersModels;
using MassTransit;

namespace AnotherService.Consumers;

public class RabbitMqConsumer : IConsumer<RabbitMqMessage>
{
    public Task Consume(ConsumeContext<RabbitMqMessage> context)
    {
        Console.WriteLine(context.Message);
        return Task.CompletedTask;
    }
}
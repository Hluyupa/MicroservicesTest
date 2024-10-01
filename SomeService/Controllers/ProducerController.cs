using BrokersModels;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace SomeService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProducerController : ControllerBase
{
    private readonly ITopicProducer<KafkaMessage> _topicProducer;
    // private readonly IPublishEndpoint _publishEndpoint;

    public ProducerController(
        ITopicProducer<KafkaMessage> topicProducer)
        // IPublishEndpoint publishEndpoint)
    {
        _topicProducer = topicProducer;
        // _publishEndpoint = publishEndpoint;
    }
    
    [HttpPost]
    public async Task<IActionResult> ProduceKafkaMessage(string messageValue)
    {
        await _topicProducer.Produce(new
        {
            Id = Guid.Parse("732d6891-074a-4753-a501-d92842862a67"),
            Text = messageValue
        });
        return Ok();
    }

    // [HttpPost("produceRabbitMqMessage")]
    // public async Task<IActionResult> ProduceRabbitMqMessage(string messageValue)
    // {
    //     await _publishEndpoint.Publish<RabbitMqMessage>(new
    //     {
    //         Text = messageValue
    //     });
    //     return Ok();
    // }
}
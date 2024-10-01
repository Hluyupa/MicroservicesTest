using BrokersModels;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;

namespace AnotherService.Consumers;

public class KafkaMessageConsumer : IConsumer<KafkaMessage>
{
    private readonly IDistributedCache _cache;
    private const string _prefix = "ProcessedKafkaMessage";
    
    public KafkaMessageConsumer(IDistributedCache cache)
    {
        _cache = cache;
    }
    
    public async Task Consume(ConsumeContext<KafkaMessage> context)
    {
        
        var message = context.Message;
        if (await IsMessageProcessed(message.Id))
        {
            return;
        }

        await CacheMessage(message);
        await File.AppendAllTextAsync("messages.txt", $"{message}\n");
    }

    private async Task CacheMessage(KafkaMessage message)
    {
        var key = _prefix + message.Id;
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
        };
        await _cache.SetStringAsync(key, "processed", options);
    }

    private async Task<bool> IsMessageProcessed(Guid messageId)
    {
        var key = _prefix + messageId;
        var value = await _cache.GetStringAsync(key);
        return value != null;
    }
}
namespace BrokersModels;

public class KafkaMessage
{
    public Guid Id { get; init; }
    
    public required string Text { get; init; }
}
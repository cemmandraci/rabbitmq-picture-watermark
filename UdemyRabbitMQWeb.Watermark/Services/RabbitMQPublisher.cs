using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace UdemyRabbitMQWeb.Watermark.Services;

public class RabbitMQPublisher
{
    private readonly RabbitMQClientService _rabbitMqClientService;

    public RabbitMQPublisher(RabbitMQClientService rabbitMqClientService)
    {
        _rabbitMqClientService = rabbitMqClientService;
    }

    public void Publish(productImageCreatedEvent productImageCreatedEvent)
    {
        var channel = _rabbitMqClientService.Connect();

        var bodyString = JsonSerializer.Serialize(productImageCreatedEvent);

        var bodyByte = Encoding.UTF8.GetBytes(bodyString);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        
        channel.BasicPublish(exchange: RabbitMQClientService.ExchangeName,
            routingKey: RabbitMQClientService.RoutingWatermark,
            basicProperties: properties,
            body:bodyByte);
        
    }
}
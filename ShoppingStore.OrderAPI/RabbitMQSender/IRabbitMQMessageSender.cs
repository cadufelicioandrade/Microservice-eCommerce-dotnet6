using ShoppingStore.MessageBus;

namespace ShoppingStore.OrderAPI.RabbitMQSender
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName);

    }
}

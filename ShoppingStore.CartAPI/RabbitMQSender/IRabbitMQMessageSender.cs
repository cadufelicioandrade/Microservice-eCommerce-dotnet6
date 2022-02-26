using ShoppingStore.MessageBus;

namespace ShoppingStore.CartAPI.RabbitMQSender
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName);

    }
}

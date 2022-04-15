using ShoppingStore.MessageBus;

namespace ShoppingStore.PaymentAPI.RabbitMQSender
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName);

    }
}

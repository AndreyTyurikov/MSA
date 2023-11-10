using System.Text;
using RabbitMQ.Client;

string topicsExchangeName = "test.topics.exchange";

ConnectionFactory rabbitConnectionFactory = new ConnectionFactory { HostName = "localhost" };

using (IConnection rabbitConnection = rabbitConnectionFactory.CreateConnection())
{
    using (IModel channel = rabbitConnection.CreateModel())
    {
        channel.ExchangeDeclare(
            exchange: topicsExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            null
            );

        long messageNumber = 1;

        while (messageNumber < 20)
        {
            string itemAddedMessage = $"Item #{messageNumber} Added";
            string itemUpdatedMessage = $"Item #{messageNumber} Updated";
            string itemDeletedMessage = $"Item #{messageNumber} Deleted";

            byte[] itemAddedBinaryBody = Encoding.Unicode.GetBytes(itemAddedMessage);
            byte[] itemUpdatedBinaryBody = Encoding.Unicode.GetBytes(itemUpdatedMessage);
            byte[] itemDeletedBinaryBody = Encoding.Unicode.GetBytes(itemDeletedMessage);

            channel.BasicPublish(
                exchange: topicsExchangeName,
                routingKey: $"item.{messageNumber}.added", // <- Exchange типа Topic использует RoutingKey для выбора очередей доставки
                mandatory: false,
                basicProperties: null,
                body: itemAddedBinaryBody
                );

            channel.BasicPublish(
                exchange: topicsExchangeName,
                routingKey: $"item.{messageNumber}.updated",
                mandatory: false,
                basicProperties: null,
                body: itemUpdatedBinaryBody
                );

            channel.BasicPublish(
                exchange: topicsExchangeName,
                routingKey: $"item.{messageNumber}.deleted",
                mandatory: false,
                basicProperties: null,
                body: itemDeletedBinaryBody
                );

            messageNumber++;
        }
    }
}
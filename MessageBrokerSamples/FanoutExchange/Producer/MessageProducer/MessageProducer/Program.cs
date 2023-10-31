using System.Text;
using RabbitMQ.Client;

//TODO: Try to create our own exchnage
string fanOutExchangeName = "custom.fanout.exchange";

ConnectionFactory rabbitConnectionFactory = new ConnectionFactory { HostName = "localhost" };

using (IConnection rabbitConnection = rabbitConnectionFactory.CreateConnection())
{
    using (IModel channel = rabbitConnection.CreateModel())
    {
        //Создадим собственный exchange
        channel.ExchangeDeclare(
            exchange: fanOutExchangeName,
            type: ExchangeType.Fanout,
            durable: true,
            autoDelete: false,
            null
            );

        long messageNumber = 1;

        while (messageNumber < messageNumber + 1000)
        {
            string message = $"Message #{messageNumber++}. For any queue of amq.fanout";

            byte[] binaryMessageBody = Encoding.Unicode.GetBytes(message);

            channel.BasicPublish(
                exchange: fanOutExchangeName,
                routingKey: "", // <- Exchange типа fanout игнорирует RoutingKey
                mandatory: false,
                basicProperties: null,
                body: binaryMessageBody
                );

            Thread.Sleep(1000);
        }
    }
}
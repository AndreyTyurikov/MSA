using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

string exchangeName = "test.topics.exchange";
string queueName = "queue.to.get.deleted.events";

ConnectionFactory rabbitConnectionFactory = new ConnectionFactory { HostName = "localhost" };

using (IConnection rabbitConnection = rabbitConnectionFactory.CreateConnection())
{
    using (IModel channel = rabbitConnection.CreateModel())
    {
        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
            );

        channel.QueueBind(
            queue: queueName,
            exchange: exchangeName,
            routingKey: "*.*.deleted", 
            arguments: null
            );

        EventingBasicConsumer channelEventsConsumer = new EventingBasicConsumer(channel);

        channelEventsConsumer.Received += (sender, args) => {

            byte[] binaryMessageBody = args.Body.ToArray();

            string stringMessageBody = Encoding.Unicode.GetString(binaryMessageBody);

            Console.WriteLine($"DeliveryTag #{args.DeliveryTag} received: {stringMessageBody}.");
            Console.WriteLine($"Exchage: {args.Exchange}. Routing Key: {args.RoutingKey}");
        };

        while (true)
        {
            channel.BasicConsume(queueName, autoAck: true, consumer: channelEventsConsumer);
        }
    }
}
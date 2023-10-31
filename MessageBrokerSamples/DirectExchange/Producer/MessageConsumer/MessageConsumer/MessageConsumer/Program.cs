using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

string firstMessageQueueName = "first.message.queue";

ConnectionFactory rabbitConnectionFactory = new ConnectionFactory { HostName = "localhost" };

using (IConnection rabbitConnection = rabbitConnectionFactory.CreateConnection())
{
    using (IModel channel = rabbitConnection.CreateModel())
    {
        channel.QueueDeclare(
            queue: firstMessageQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
            );

        //Читалка сообщений из канала
        EventingBasicConsumer channelEventsConsumer = new EventingBasicConsumer(channel);

        channelEventsConsumer.Received += (sender, args) => {
        
            byte[] binaryMessageBody = args.Body.ToArray();

            string stringMessageBody =  Encoding.Unicode.GetString(binaryMessageBody);

            Console.WriteLine($"Message #{args.DeliveryTag} received: {stringMessageBody}.");
            Console.WriteLine($"Exchage: {args.Exchange}. Routing Key (Queue Name): {args.RoutingKey}");

            Thread.Sleep(1000);
        };

        while (true)
        {
            channel.BasicConsume(firstMessageQueueName, autoAck: true, consumer: channelEventsConsumer);
        }

    }
}

using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

string fanOutExchangeName = "custom.fanout.exchange";

ConnectionFactory rabbitConnectionFactory = new ConnectionFactory { HostName = "localhost" };

using (IConnection rabbitConnection = rabbitConnectionFactory.CreateConnection())
{
    using (IModel channel = rabbitConnection.CreateModel())
    {
        //Объявляем очередь со случайным именем
        QueueDeclareOk queueDeclareResult = channel.QueueDeclare(
            queue: "",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
            );

        //Привязываем вручную очередь к нужному обменнику
        channel.QueueBind(queueDeclareResult.QueueName, fanOutExchangeName, "");

        //Читалка сообщений из канала
        EventingBasicConsumer channelEventsConsumer = new EventingBasicConsumer(channel);

        channelEventsConsumer.Received += (sender, args) => {

            byte[] binaryMessageBody = args.Body.ToArray();

            string stringMessageBody = Encoding.Unicode.GetString(binaryMessageBody);

            Console.WriteLine($"DeliveryTag #{args.DeliveryTag} received: {stringMessageBody}.");
            Console.WriteLine($"Exchage: {args.Exchange}. Routing Key (Queue Name): {args.RoutingKey}");

            Thread.Sleep(1000);
        };

        while (true)
        {
            channel.BasicConsume(queueDeclareResult.QueueName, autoAck: true, consumer: channelEventsConsumer);
        }

    }
}

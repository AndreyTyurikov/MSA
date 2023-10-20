using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

string firstMessageQueueName = "first.message.queue";
//string secondMessageQueueName = "second.message.queue";

ConnectionFactory rabbitConnectionFactory = new ConnectionFactory { HostName = "localhost" };

using (IConnection rabbitConnection = rabbitConnectionFactory.CreateConnection())
{
    using (IModel channel = rabbitConnection.CreateModel())
    {
        #region messageFromFirstQueue
        channel.QueueDeclare(
            queue: firstMessageQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null  //Без. доп. опций. Все настройки очереди по-умолчанию
            );

        //Читалка сообщений из канала
        EventingBasicConsumer channelEventsConsumer = new EventingBasicConsumer(channel);

        channelEventsConsumer.Received += (sender, args) => {
        
            byte[] binaryMessageBody = args.Body.ToArray();

            string stringMessageBody =  Encoding.Unicode.GetString(binaryMessageBody);

            Console.WriteLine($"Message #{args.DeliveryTag} received: {stringMessageBody}.");
            Console.WriteLine($"Exchage: {args.Exchange}. Routing Key (Queue Name): {args.RoutingKey}");
                
        };

        //TODO!
        //1. Read and save delivery Tag
        //2. Read and save Routing Key (Queue name)

        //TODO: Get rid of hardcoded queue
        channel.BasicConsume(queue: firstMessageQueueName, autoAck: false, consumer: channelEventsConsumer);
        //TODO: Get rid of hardcoded deliveryTag
        channel.BasicAck(deliveryTag: 1, multiple: false);

        #endregion

        Console.ReadLine();
    }
}

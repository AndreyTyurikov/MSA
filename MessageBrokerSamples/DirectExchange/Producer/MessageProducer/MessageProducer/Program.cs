using System.Text;
using RabbitMQ.Client;

string firstMessageQueueName = "first.message.queue";
string secondMessageQueueName = "second.message.queue";

ConnectionFactory rabbitConnectionFactory = new ConnectionFactory { HostName = "localhost" };

using (IConnection rabbitConnection = rabbitConnectionFactory.CreateConnection())
{
    using (IModel channel = rabbitConnection.CreateModel())
    {
        //Эта очередь будет присоединена к обменнику по-умолчанию (Default Exchange)
        channel.QueueDeclare(
            queue: firstMessageQueueName,
            durable: true,
            exclusive: false, //TODO: Change it to false on testing 
            autoDelete: false,
            arguments: null  //Без. доп. опций. Все настройки очереди по-умолчанию
            );

        string firstTestMessage = "Message #1. Hello buddy. What's up";

        //AMPQ принимает только двоичные сообщения. 
        byte[] binaryMessageBody = Encoding.Unicode.GetBytes(firstTestMessage);

        //Отправляем сообщение
        channel.BasicPublish(
            exchange: string.Empty, // <--- Будет использован обменник по-умолчанию (AMQP default)
            routingKey: firstMessageQueueName, // <-- В режиме Direct Exchange чтобы сообщение попало в нужную очередь, 
                                               // его routingKey должен быть равен имени самой очереди
            mandatory: false,                  // В случае проблем с дооставкой - не возвращать отправителю. Уничтожать (drop). 
            basicProperties: null,
            body: binaryMessageBody
            );
    }
}


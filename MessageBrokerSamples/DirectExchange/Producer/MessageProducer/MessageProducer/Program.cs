using System.Text;
using RabbitMQ.Client;

string firstMessageQueueName = "first.message.queue";

ConnectionFactory rabbitConnectionFactory = new ConnectionFactory { HostName = "localhost" };

using (IConnection rabbitConnection = rabbitConnectionFactory.CreateConnection())
{
    using (IModel channel = rabbitConnection.CreateModel())
    {
        #region messageToFirstQueue

        channel.QueueDeclare(
            queue: firstMessageQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null 
            );

        long messageNumber = 1;

        while (messageNumber < 20000)
        {
            string message = $"Message #{messageNumber++}. For queue: first.message.queue";

            byte[] binaryMessageBody = Encoding.Unicode.GetBytes(message);

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: firstMessageQueueName,
                mandatory: false,
                basicProperties: null,
                body: binaryMessageBody
                );
        }

        #endregion
    }
}


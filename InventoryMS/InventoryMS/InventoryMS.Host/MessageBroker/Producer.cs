using System.Text;
using System.Text.Json;
using InventoryMS.Events;
using RabbitMQ.Client;

namespace InventoryMS.Host.MessageBroker
{
    public class Producer : IMessageBusProducer
    {
        private ConnectionFactory rabbitConnectionFactory = new ConnectionFactory { HostName = "localhost" };

        public bool PublishEvent(InventoryMsEvent eventToPublish)
        {
            try
            {
                using (IConnection rabbitConnection = rabbitConnectionFactory.CreateConnection())
                {
                    using (IModel channel = rabbitConnection.CreateModel())
                    {
                        channel.ExchangeDeclare(
                            exchange: InventoryMS.Events.Constants.ExchangeName,
                            type: ExchangeType.Fanout,
                            durable: true,
                            autoDelete: false,
                            null
                            );

                        string eventJson = JsonSerializer.Serialize(eventToPublish);
                        byte[] binaryEventBody = Encoding.UTF8.GetBytes(eventJson);

                        channel.BasicPublish(
                            exchange: InventoryMS.Events.Constants.ExchangeName,
                            routingKey: "",
                            mandatory: false,
                            basicProperties: null,
                            body: binaryEventBody
                            );
                    }
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

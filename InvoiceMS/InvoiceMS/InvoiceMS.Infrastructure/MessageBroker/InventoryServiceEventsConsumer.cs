using InventoryMS.Events;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace InvoiceMS.Infrastructure.MessageBroker
{
    public class InventoryServiceEventsConsumer : IHostedService
    {
        private Timer? _timer = null;

        string messageQueueName = "inventory_ms.events.for.invoice_ms";
        private ConnectionFactory rabbitConnectionFactory = new ConnectionFactory { HostName = "localhost" };
        private readonly IConnection rabbitMQConnection;
        private readonly IModel rabbitMQChannel;
        private EventingBasicConsumer channelEventsConsumer;
        private readonly IInvetoryServiceEventsProcessor _invetoryServiceEventsProcessor;

        public InventoryServiceEventsConsumer(IInvetoryServiceEventsProcessor invetoryServiceEventsProcessor)
        {
            _invetoryServiceEventsProcessor = invetoryServiceEventsProcessor;

            try
            {
                rabbitMQConnection = rabbitConnectionFactory.CreateConnection();
                rabbitMQChannel = rabbitMQConnection.CreateModel();

                rabbitMQChannel.QueueDeclare(
                    queue: messageQueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                    );

                rabbitMQChannel.QueueBind(
                    messageQueueName, 
                    InventoryMS.Events.Constants.ExchangeName, 
                    "" //<-- Routing key is empty cause Exchange type is fanout
                    );

                channelEventsConsumer = new EventingBasicConsumer(rabbitMQChannel);
                SetupEventsConsumer(channelEventsConsumer);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SetupEventsConsumer(EventingBasicConsumer channelEventsConsumer)
        {
            channelEventsConsumer.Received += (sender, args) =>
            {
                byte[] binaryMessageBody = args.Body.ToArray();
                string JSONMessageBody = Encoding.UTF8.GetString(binaryMessageBody);

                try
                {
                    InventoryMsEvent? eventReceived = JsonSerializer.Deserialize<InventoryMsEvent>(JSONMessageBody);

                    if (eventReceived != null)
                    {
                        _invetoryServiceEventsProcessor.ProcessEvent(eventReceived);
                    }

                }
                catch (Exception)
                {
                    Debug.WriteLine("Error processing incoming message");

                    throw;
                }

            };
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ConsumeEventBusMessages, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        void ConsumeEventBusMessages(object? state)
        {
            rabbitMQChannel.BasicConsume(messageQueueName, autoAck: true, consumer: channelEventsConsumer);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}


using RabbitMQ.Client;

namespace TrainApp.Core.ApplicationService.RabbitMq
{
    public class RabbitMqService
    {
        //Mail Transfer Agent
        public static readonly string SerialisationQueueName = "queue_Notify";
        public static readonly string SerialisationExchangeName = "exchange_Notify";
        public static readonly string SerialisationRoutingKey = "routingKey_Notify";

        private static volatile IConnection _connection;
        private static readonly object ConnectionLock = new object();
        private static volatile IModel _channel;
        private static readonly object ChannelLock = new object();

        public static IConnection RabbitMqConnection
        {
            get
            {
                if (_connection != null) { return _connection; }
                lock (ConnectionLock)
                {
                    if (_connection != null) { return _connection; }

                    var connectionFactory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest", AutomaticRecoveryEnabled = true };
                    _connection = connectionFactory.CreateConnection();
                }
                return _connection;
            }
        }

        public static IModel RabbitMqModel
        {
            get
            {
                if (_channel != null) { return _channel; }
                lock (ChannelLock)
                {
                    if (_channel != null) { return _channel; }
                    _channel = RabbitMqConnection.CreateModel();
                }
                return _channel;
            }
        }

        public static void SetupInitialTopicQueue(IModel model)
        {
            model.QueueDeclare(SerialisationQueueName, durable: true, exclusive: false, autoDelete: false);
            model.ExchangeDeclare(SerialisationExchangeName, ExchangeType.Direct);
            model.QueueBind(SerialisationQueueName, SerialisationExchangeName, SerialisationRoutingKey);
        }

    }
}

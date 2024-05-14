//using RabbitMQ.Client;
//using System;
//using System.Text;
//using Microsoft.Extensions.Logging;

//namespace DLS_Catalog_Service.Services
//{
//    public class RabbitMQService : IDisposable
//    {
//        private readonly ConnectionFactory _factory;
//        private readonly ILogger _logger;
//        private IConnection _connection;
//        private IModel _channel;

//        public RabbitMQService(ConnectionFactory connectionFactory, ILogger<RabbitMQService> logger)
//        {
//            _factory = connectionFactory;
//            _logger = logger;

//            try
//            {
//                _connection = _factory.CreateConnection();
//                _channel = _connection.CreateModel();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Could not connect to RabbitMQ");
//                throw;
//            }
//        }

//        public void SendMessage(string queueName, string message)
//        {
//            _channel.QueueDeclare(queue: queueName,
//                durable: false,
//                exclusive: false,
//                autoDelete: false,
//                arguments: null);

//            var body = Encoding.UTF8.GetBytes(message);
//            _channel.BasicPublish(exchange: "",
//                routingKey: queueName,
//                basicProperties: null,
//                body: body);
//        }

//        public void Dispose()
//        {
//            _channel?.Close();
//            _connection?.Close();
//        }
//    }
//}
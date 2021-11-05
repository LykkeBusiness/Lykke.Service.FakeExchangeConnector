using System;
using System.Threading.Tasks;
using Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber.Deserializers;
using Lykke.RabbitMqBroker.Subscriber.MessageReadStrategies;
using Lykke.RabbitMqBroker.Subscriber.Middleware.ErrorHandling;
using Lykke.Service.FakeExchangeConnector.Core.Domain;
using Lykke.Service.FakeExchangeConnector.Core.Rabbit;
using Lykke.Snow.Common.Correlation.RabbitMq;
using Microsoft.Extensions.Logging;

namespace Lykke.Service.FakeExchangeConnector.RabbitSubscribers
{
    public class RabbitMqSubscriber<T> : IRabbitMqSubscriber<T>, IDisposable
    {
        private readonly ILog _log;
        
        private RabbitMqBroker.Subscriber.RabbitMqSubscriber<T> _subscriber;
        private readonly IMessageDeserializer<T> _messageDeserializer;
        private readonly RabbitMqCorrelationManager _correlationManager;
        private readonly ILoggerFactory _loggerFactory;
        
        private readonly string _connectionString;
        private readonly string _exchangeName;
        private readonly string _queueName;
        private readonly bool _isDurable;

        public RabbitMqSubscriber(
            RabbitMqCorrelationManager correlationManager,
            ILoggerFactory loggerFactory,
            ILog log,
            string connectionString,
            string exchangeName,
            string queueName, 
            bool isDurable,
            string messageFormat)
        {
            _log = log;
            _connectionString = connectionString;
            _exchangeName = exchangeName;
            _queueName = queueName;
            _isDurable = isDurable;
            _correlationManager = correlationManager;
            _loggerFactory = loggerFactory;

            _messageDeserializer = GetDeserializer(Enum.TryParse<RabbitMessageFormat>(messageFormat, out var format)
                ? format
                : default);
        }

        public void Subscribe(Func<T, Task> handleMessage)
        {
            var settings = new RabbitMqSubscriptionSettings
            {
                ConnectionString = _connectionString,
                ExchangeName = _exchangeName,
                QueueName = _queueName,
                IsDurable = _isDurable,
            };

            _subscriber = new RabbitMqBroker.Subscriber.RabbitMqSubscriber<T>(
                    _loggerFactory.CreateLogger<RabbitMqBroker.Subscriber.RabbitMqSubscriber<T>>(),
                    settings)
                .SetMessageDeserializer(_messageDeserializer)
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(handleMessage)
                .CreateDefaultBinding()
                .UseMiddleware(new ExceptionSwallowMiddleware<T>(
                    _loggerFactory.CreateLogger<ExceptionSwallowMiddleware<T>>()))
                .UseMiddleware(new ResilientErrorHandlingMiddleware<T>(
                    _loggerFactory.CreateLogger<ResilientErrorHandlingMiddleware<T>>(),
                    TimeSpan.FromSeconds(10)))
                .SetReadHeadersAction(_correlationManager.FetchCorrelationIfExists)
                .Start();
        }

        public void Dispose()
        {
            _subscriber?.Dispose();
        }

        private IMessageDeserializer<T> GetDeserializer(RabbitMessageFormat format)
        {
            switch (format)
            {
                case RabbitMessageFormat.Json:
                    return new JsonMessageDeserializer<T>();
                case RabbitMessageFormat.MessagePack:
                    return new MessagePackMessageDeserializer<T>();
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }
    }
}

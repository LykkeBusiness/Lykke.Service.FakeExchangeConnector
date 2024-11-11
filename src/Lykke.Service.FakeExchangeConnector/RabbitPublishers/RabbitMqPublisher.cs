using System;
using System.Threading.Tasks;
using Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.RabbitMqBroker.Publisher.Serializers;
using Lykke.RabbitMqBroker.Publisher.Strategies;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.FakeExchangeConnector.Core;
using Lykke.Service.FakeExchangeConnector.Core.Domain;
using Lykke.Service.FakeExchangeConnector.Core.Rabbit;
using Lykke.Snow.Common.Correlation.RabbitMq;
using MessagePack;
using Microsoft.Extensions.Logging;

namespace Lykke.Service.FakeExchangeConnector.RabbitPublishers
{
    public class RabbitMqPublisher<T> : IRabbitMqPublisher<T>, IDisposable
    {
        private readonly bool _enabled;
        private readonly RabbitMqBroker.Publisher.RabbitMqPublisher<T> _rabbitPublisher;
        private readonly object _sync = new object();

        public RabbitMqPublisher(
            RabbitMqCorrelationManager correlationManager,
            ILoggerFactory loggerFactory,
            string connectionString,
            string exchangeName,
            bool enabled,
            ILog log, 
            bool durable = true, 
            string messageFormat = null)
        {
            _enabled = enabled;
            if (!enabled)
            {
                log.WriteInfoAsync($"{GetType()}", "Constructor", $"A rabbit mq handler for {typeof(T)} is disabled");
                return;
            }
            var publisherSettings = new RabbitMqSubscriptionSettings
            {
                ConnectionString = connectionString,
                ExchangeName = exchangeName,
                IsDurable = durable
            };

            var serializer = GetSerializer(Enum.TryParse<RabbitMessageFormat>(messageFormat, out var format)
                ? format
                : default); 

            _rabbitPublisher = new RabbitMqBroker.Publisher.RabbitMqPublisher<T>(loggerFactory, publisherSettings)
                .DisableInMemoryQueuePersistence()
                .SetSerializer(serializer)
                .SetWriteHeadersFunc(correlationManager.BuildCorrelationHeadersIfExists)
                .SetPublishStrategy(new DefaultFanoutPublishStrategy(publisherSettings))
                .PublishSynchronously();
            _rabbitPublisher.Start();
        }

        public Task Publish(T message)
        {
            if (!_enabled)
            {
                return Task.CompletedTask;
            }
            lock (_sync)
            {
                return _rabbitPublisher.ProduceAsync(message);
            }
        }

        public void Dispose()
        {
            _rabbitPublisher?.Stop();
            _rabbitPublisher?.Dispose();
        }

        private IRabbitMqSerializer<T> GetSerializer(RabbitMessageFormat format)
        {
            switch (format)
            {
                case RabbitMessageFormat.Json:
                    return new JsonMessageSerializer<T>();
                case RabbitMessageFormat.MessagePack:
                    return new MessagePackMessageSerializer<T>((IFormatterResolver)null);
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }
    }
}

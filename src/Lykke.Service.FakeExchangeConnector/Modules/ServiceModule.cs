using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Log;
using Lykke.Service.FakeExchangeConnector.Core.Caches;
using Lykke.Service.FakeExchangeConnector.Core.Rabbit;
using Lykke.Service.FakeExchangeConnector.Core.Services;
using Lykke.Service.FakeExchangeConnector.Core.Settings.ServiceSettings;
using Lykke.Service.FakeExchangeConnector.PeriodicalHandlers;
using Lykke.Service.FakeExchangeConnector.RabbitPublishers;
using Lykke.Service.FakeExchangeConnector.RabbitSubscribers;
using Lykke.Service.FakeExchangeConnector.Services;
using Lykke.Service.FakeExchangeConnector.Services.Caches;
using Lykke.Service.FakeExchangeConnector.Services.Services;
using Lykke.SettingsReader;
using Lykke.Snow.Common.Correlation.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Lykke.Service.FakeExchangeConnector.Modules
{
    public class ServiceModule : Module
    {
        private readonly FakeExchangeConnectorSettings _settings;
        private readonly ILog _log;
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies
        private readonly IServiceCollection _services;

        public ServiceModule(
            IReloadingManager<FakeExchangeConnectorSettings> settings,
            ILog log)
        {
            _settings = settings.CurrentValue;
            _log = log;

            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            // TODO: Do not register entire settings in container, pass necessary settings to services which requires them
            // ex:
            //  builder.RegisterType<QuotesPublisher>()
            //      .As<IQuotesPublisher>()
            //      .WithParameter(TypedParameter.From(_settings.CurrentValue.QuotesPublication))

            builder.RegisterInstance(_settings)
                .AsSelf()
                .SingleInstance();
            
            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            // TODO: Add your dependencies here

            builder.RegisterType<ExchangeCache>()
                .As<IExchangeCache>()
                .SingleInstance();

            builder.RegisterType<OrderBookCache>()
                .As<IOrderBookCache>()
                .SingleInstance();
            
            builder.RegisterType<QuoteCache>()
                .As<IQuoteCache>()
                .SingleInstance();

            builder.RegisterType<TradingService>()
                .As<ITradingService>()
                .SingleInstance();

            builder.RegisterType<OrderBookService>()
                .As<IOrderBookService>()
                .SingleInstance();

            builder.RegisterType<QuoteService>()
                .As<IQuoteService>()
                .SingleInstance();
            
            RegisterPeriodicalHandlers(builder);
            
            RegisterRabbitMqSubscribers(builder);

            RegisterRabbitMqPublishers(builder);

            builder.Populate(_services);
        }

        private void RegisterPeriodicalHandlers(ContainerBuilder builder)
        {
            builder.RegisterType<FakeOrderBookHandler>()
                .WithParameter(TypedParameter.From(_settings.FakeOrderBookPublishingPeriodMilliseconds))
                .SingleInstance();
        }
        
        private void RegisterRabbitMqSubscribers(ContainerBuilder builder)
        {
            builder.RegisterType<OrderBookSubscriber>()
                .AsSelf()
                .SingleInstance()
                .WithParameter((pi, c) => pi.Name == "correlationManager",
                    (pi, c) => c.Resolve<RabbitMqCorrelationManager>())
                .WithParameter((pi, c) => pi.Name == "loggerFactory",
                    (pi, c) => c.Resolve<ILoggerFactory>())
                .WithParameters(new[]
                {
                    new NamedParameter("connectionString", _settings.Rabbit.ExchangeConnectorQuotes.ConnectionString),
                    new NamedParameter("exchangeName", _settings.Rabbit.ExchangeConnectorQuotes.ExchangeName),
                    new NamedParameter("queueName", _settings.Rabbit.ExchangeConnectorQuotes.QueueName),
                    new NamedParameter("isDurable", false),
                    new NamedParameter("log", _log),
                    new NamedParameter("messageFormat", _settings.Rabbit.ExchangeConnectorQuotes.MessageFormat), 
                });
        }

        private void RegisterRabbitMqPublishers(ContainerBuilder builder)
        {
            builder.RegisterType<ExecutionReportPublisher>()
                .As<IExecutionReportPublisher>()
                .SingleInstance()
                .WithParameter((pi, c) => pi.Name == "correlationManager",
                    (pi, c) => c.Resolve<RabbitMqCorrelationManager>())
                .WithParameter((pi, c) => pi.Name == "loggerFactory",
                    (pi, c) => c.Resolve<ILoggerFactory>())
                .WithParameters(new[]
                {
                    new NamedParameter("connectionString", _settings.Rabbit.ExchangeConnectorOrder.ConnectionString),
                    new NamedParameter("exchangeName", _settings.Rabbit.ExchangeConnectorOrder.ExchangeName),
                    new NamedParameter("enabled", true),
                    new NamedParameter("log", _log),
                    new NamedParameter("messageFormat", _settings.Rabbit.ExchangeConnectorOrder.MessageFormat), 
                });
            
            builder.RegisterType<FakeOrderBookFakePublisher>()
                .As<IFakeOrderBookPublisher>()
                .SingleInstance()
                .WithParameter((pi, c) => pi.Name == "correlationManager",
                    (pi, c) => c.Resolve<RabbitMqCorrelationManager>())
                .WithParameter((pi, c) => pi.Name == "loggerFactory",
                    (pi, c) => c.Resolve<ILoggerFactory>())
                .WithParameters(new[]
                {
                    new NamedParameter("connectionString", _settings.Rabbit.FakeOrderBook.ConnectionString),
                    new NamedParameter("exchangeName", _settings.Rabbit.FakeOrderBook.ExchangeName),
                    new NamedParameter("enabled", true),
                    new NamedParameter("log", _log),
                    new NamedParameter("messageFormat", _settings.Rabbit.FakeOrderBook.MessageFormat), 
                });
        }
    }
}

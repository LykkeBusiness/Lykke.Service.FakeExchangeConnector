using System;
using Autofac;
using Common.Log;

namespace Lykke.Service.FakeExchangeConnector.Client
{
    public static class AutofacExtension
    {
        public static void RegisterFakeExchangeConnectorClient(this ContainerBuilder builder, string serviceUrl, ILog log)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (serviceUrl == null) throw new ArgumentNullException(nameof(serviceUrl));
            if (log == null) throw new ArgumentNullException(nameof(log));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));

            builder.RegisterType<FakeExchangeConnectorClient>()
                .WithParameter("serviceUrl", serviceUrl)
                .As<IFakeExchangeConnectorClient>()
                .SingleInstance();
        }

        public static void RegisterFakeExchangeConnectorClient(this ContainerBuilder builder, FakeExchangeConnectorServiceClientSettings settings, ILog log)
        {
            builder.RegisterFakeExchangeConnectorClient(settings?.ServiceUrl, log);
        }
    }
}

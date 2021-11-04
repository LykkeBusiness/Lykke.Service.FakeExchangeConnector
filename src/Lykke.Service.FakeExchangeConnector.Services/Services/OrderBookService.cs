using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.FakeExchangeConnector.Core.Caches;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;
using Lykke.Service.FakeExchangeConnector.Core.Rabbit;
using Lykke.Service.FakeExchangeConnector.Core.Services;
using Lykke.Service.FakeExchangeConnector.Core.Settings.ServiceSettings;
using Lykke.Snow.Common.Correlation;

namespace Lykke.Service.FakeExchangeConnector.Services.Services
{
    public class OrderBookService : IOrderBookService
    {
        private readonly IOrderBookCache _orderBookCache;
        private readonly IFakeOrderBookPublisher _fakeOrderBookPublisher;
        private readonly FakeExchangeConnectorSettings _fakeExchangeConnectorSettings;
        private readonly CorrelationContextAccessor _correlationContextAccessor;
        private readonly ILog _log;

        public OrderBookService(IOrderBookCache orderBookCache,
            IFakeOrderBookPublisher fakeOrderBookPublisher,
            FakeExchangeConnectorSettings fakeExchangeConnectorSettings,
            CorrelationContextAccessor correlationContextAccessor,
            ILog log)
        {
            _orderBookCache = orderBookCache;
            _fakeOrderBookPublisher = fakeOrderBookPublisher;
            _fakeExchangeConnectorSettings = fakeExchangeConnectorSettings;
            _correlationContextAccessor = correlationContextAccessor;
            _log = log;
        }

        public async Task PostFakeOrderBooks()
        {
            var orderbooks = ShakeBooks(_orderBookCache.GetAll());

            await Task.WhenAll(orderbooks.Select(x =>
            {
                var correlationId = $"publish-orderbook-{x.AssetPairId}-{Guid.NewGuid().ToString("N")}";
                _correlationContextAccessor.CorrelationContext = new CorrelationContext(correlationId);
                _log.WriteMonitor( nameof(PostFakeOrderBooks),nameof(OrderBookService),$"Correlation context with id '{correlationId}' was created.");

                return _fakeOrderBookPublisher.Publish(x);
            }));
        }

        public void RemoveOrderBooksByAssetPair(string assetPairId)
        {
            if (string.IsNullOrWhiteSpace(assetPairId))
                throw new ArgumentNullException(nameof(assetPairId));
            
            _orderBookCache.ClearByCondition(o => o.AssetPairId == assetPairId);
        }

        private IEnumerable<OrderBook> ShakeBooks(IEnumerable<OrderBook> orderBooks)
        {
            var random = new Random();
            var delta = (decimal) random.NextDouble();
            return orderBooks.Select(x =>
                    new OrderBook(x.Source, x.AssetPairId, ShakePrices(x.Asks, delta), ShakePrices(x.Bids, delta), x.Timestamp))
                .ToList();
        }

        private List<VolumePrice> ShakePrices(IEnumerable<VolumePrice> book, decimal delta)
        {
            return book.Select(x =>
                    new VolumePrice(Math.Round(x.Price *
                                               (1 + _fakeExchangeConnectorSettings.OrderBookDeltaPercentage * delta),
                        _fakeExchangeConnectorSettings.PriceAccuracy), x.Volume))
                .ToList();
        }
    }
}

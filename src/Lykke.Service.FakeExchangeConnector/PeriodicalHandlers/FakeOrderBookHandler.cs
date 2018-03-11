using System;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Service.FakeExchangeConnector.Core.Services;

namespace Lykke.Service.FakeExchangeConnector.PeriodicalHandlers
{
    public class FakeOrderBookHandler : TimerPeriod
    {
        private readonly IOrderBookService _orderBookService;
        
        public FakeOrderBookHandler(int periodMilliseconds,
            ILog log,
            IOrderBookService orderBookService)
            : base(nameof(FakeOrderBookHandler), periodMilliseconds, log)
        {
            _orderBookService = orderBookService;
        }

        public override async Task Execute()
        {
            await _orderBookService.PostFakeOrderBooks();
        }
    }
}

namespace Lykke.Service.FakeExchangeConnector.Core.Domain.Trading
{
    public class AccountBalance
    {
        public string Asset { get; set; }
        
        public decimal Balance { get; set; }
        
        public AccountBalance Clone()
        {
            return new AccountBalance
            {
                Asset = this.Asset,
                Balance = this.Balance
            };
        }
    }
}

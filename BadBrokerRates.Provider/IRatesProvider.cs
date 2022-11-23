namespace BadBrokerRates.Providers
{
    public interface IRatesProvider
    {
        Task<Dictionary<DateTime, Dictionary<string, decimal>>> GetCurrencyRates(DateTime Start, DateTime End);

    }
}

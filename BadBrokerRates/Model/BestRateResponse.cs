namespace BadBrokerRates.Model
{
    public class BestRateResponse
    {
        public List<Dictionary<string, object>> Rates { get; set; }
        public DateTime BuyDate { get; set; }
        public DateTime SellDate { get; set; }
        public string Tool { get; set; }
        public decimal Revenue { get; set; }
    }
}

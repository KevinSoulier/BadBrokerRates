using BadBrokerRates.Providers;
using BadBrokerRates.Providers.Models;
using BadBrokerRates.Service.Model;
using BadBrokerRates.Services;
using ExchangeRates.Client;

namespace BadBrokerRates.Service
{
    public class RateService : IRateService
    {
        IRatesProvider _ratesProvider;
        IExchangeRatesClient _exchangeRatesClient;

        public RateService(IRatesProvider RatesProvider)
        {
            _ratesProvider = RatesProvider;
        }

        public async Task<BestRate> GetBestRate(DateTime Start, DateTime End, decimal Amount)
        {
            BestRate bestRate = new BestRate();

            List<string> currencies = Enum.GetNames(typeof(Currency)).ToList();
            currencies.Remove(Currency.USD.ToString());

            string outCurr = "";
            string comma = "";
            foreach (var curr in currencies)
            {
                outCurr = outCurr + comma + curr;
                comma = ",";
            }

            var rates = await _ratesProvider.GetCurrencyRates(Start, End);

            //Array {curr, revenue, start end}

            List<object[]> revenueByCurr = new List<object[]>();

            foreach (var curr in currencies)
            {
                var ratesByCurr = rates.Select(x => new KeyValuePair<DateTime, Decimal>(x.Key, x.Value[curr]))
                    .ToDictionary(x => x.Key, x => x.Value);
                DateTime start;
                DateTime end;

                decimal revenue = CalculateBestRate(ratesByCurr, out start, out end, Amount);

                revenueByCurr.Add(new object[] { curr, revenue, start, end });
            }

            object[] bestCurrRevenue = revenueByCurr.MaxBy(x => ((decimal)x[1]));

            bestRate.Tool = (string)bestCurrRevenue[0];
            bestRate.Revenue = System.Math.Round((decimal)bestCurrRevenue[1], 2);
            bestRate.BuyDate = (DateTime)bestCurrRevenue[2];
            bestRate.SellDate = (DateTime)bestCurrRevenue[3];

            rates = rates.ToDictionary(x => x.Key, x => x.Value.ToDictionary(y => y.Key, y => System.Math.Round(y.Value, 2)));

            bestRate.Rates = rates;

            return bestRate;
        }

        private decimal CalculateBestRate(Dictionary<DateTime, decimal> Rates, out DateTime Start, out DateTime End, decimal DollarAmount)
        {

            List<DateTime[]> ranges = new List<DateTime[]>(); //every array in the list has two values Start and End
            List<decimal> revenues = new List<decimal>(); //every revenew in the list is at the same index of the rages list

            KeyValuePair<DateTime, Decimal>[] arrRates = Rates.OrderBy(x => x.Key).ToArray();

            for (int i = 0; i < arrRates.Length; i++)
            {
                DateTime start = arrRates[i].Key;
                for (int j = (i + 1); j < arrRates.Length; j++)
                {
                    DateTime end = arrRates[j].Key;

                    decimal periodRevenue = ((arrRates[i].Value * DollarAmount) / arrRates[j].Value) - (end - start).Days;

                    DateTime[] range = new DateTime[] { start, end };
                    ranges.Add(range);
                    revenues.Add(periodRevenue);
                }
            }

            decimal bestRevenue;
            bestRevenue = revenues.Max();
            int bestRevenueIndex = revenues.IndexOf(bestRevenue);

            Start = ranges[bestRevenueIndex][0];
            End = ranges[bestRevenueIndex][1];

            return bestRevenue;
        }
    }
}

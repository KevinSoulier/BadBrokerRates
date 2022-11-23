using BadBrokerRates.Providers.Models;
using BadBrokerRates.Repository;
using ExchangeRates.Client;

namespace BadBrokerRates.Providers
{
    public class RatesProvider : IRatesProvider
    {
        BadBrokerRatesRepositoryContext _repository;
        IExchangeRatesClient _exchangeRatesClient;

        public RatesProvider(BadBrokerRatesRepositoryContext Repository, IExchangeRatesClient ExchangeRatesClient)
        {
            _repository = Repository;
            _exchangeRatesClient = ExchangeRatesClient;
        }
        public async Task<Dictionary<DateTime, Dictionary<string, decimal>>> GetCurrencyRates(DateTime Start, DateTime End)
        {
            Dictionary<DateTime, Dictionary<string, decimal>> results;

            //look in db if the requeste range exist.
            Dictionary<string, Dictionary<DateTime, decimal>> resultsPerCurr = new Dictionary<string, Dictionary<DateTime, decimal>>();

            //look ranges in database for each currency handled by the app
            results = GetFromDb(Start, End);

            List<string> currencies = Enum.GetNames(typeof(Currency)).ToList();
            currencies.Remove(Currency.USD.ToString());

            //look up the missing days
            DateTime startToQuery = End;
            DateTime endToQuery = Start;
            bool callApiFlag = false;

            //has all dates?
            for (DateTime date = Start; date.Date <= End.Date; date = date.AddDays(1))
            {
                //if not contains the given day save to call the api later
                if (!results.ContainsKey(date) || !currencies.All(x => results[date].ContainsKey(x)))
                {
                    callApiFlag = true;
                    if (date < startToQuery)
                    {
                        startToQuery = date;
                    }
                    if (date > endToQuery)
                    {
                        endToQuery = date;
                    }
                }
            }

            if (callApiFlag)
            {
                var apiResult = await GetFromApi(startToQuery, endToQuery);

                apiResult.ToList().ForEach(x =>
                {
                    // for each date in api result, look if exist in db result
                    if (results.ContainsKey(x.Key))
                    {
                        x.Value.ToList().ForEach(y =>
                        {
                            //for each curr in the given date look if exist in the db result
                            if (!results[x.Key].ContainsKey(y.Key))
                            {
                                results[x.Key].Add(y.Key, y.Value);
                            }
                        });
                    }
                    else
                    {
                        results.Add(x.Key, x.Value);
                    }
                });
                SaveInRepository(apiResult);
            }
            return results.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }

        private Dictionary<DateTime, Dictionary<string, decimal>> GetFromDb(DateTime Start, DateTime End)
        {            
            Dictionary<DateTime, Dictionary<string, decimal>> results = new Dictionary<DateTime, Dictionary<string, decimal>>();
            try
            {
                var resultsDb = _repository.CurrencyRates.Where(x => x.Date >= Start && x.Date <= End).ToList();

                var allDates = resultsDb.Select(x => x.Date).Distinct();
                
                results = allDates.ToDictionary(x => x.Date, x => resultsDb.Where(y => y.Date.Equals(x.Date)).ToDictionary(y => y.CurrencyCode, y => y.Amount));

                //.DistinctBy(y => y.Currency.Code)
            }
            catch(Exception e)
            {
                throw new Exception("Error Getting rates from DB", e);
            }

            return results;
        }

        private async Task<Dictionary<DateTime, Dictionary<string,Decimal>>> GetFromApi(DateTime Start, DateTime End)
        {
            List<string> currencies = Enum.GetNames(typeof(Currency)).ToList();
            currencies.Remove(Currency.USD.ToString());

            string outCurr = "";
            string comma = "";
            foreach (var curr in currencies)
            {
                outCurr = outCurr + comma + curr;
                comma = ",";
            }

            var apiResults = await _exchangeRatesClient.GetTimeSeries(Start, End, Currency.USD.ToString(), outCurr);
            var apiResultFiltered = apiResults.Rates.ToDictionary(x => x.Key, x => x.Value.Where(y => currencies.Contains(y.Key)).ToDictionary(y => y.Key, y => y.Value));

            return apiResultFiltered;
        }

        private void SaveInRepository(Dictionary<DateTime, Dictionary<string, decimal>> RatesToSave)
        {
            try
            {
                List<string> currencies = Enum.GetNames(typeof(Currency)).ToList();
                currencies.Remove(Currency.USD.ToString());

                var allRatesInDb = _repository.CurrencyRates.ToList();

                foreach(var date in RatesToSave)
                {
                    foreach (var currRate in date.Value)
                    {
                        if (!allRatesInDb.Exists(x=> x.Date.Equals(date.Key) && x.CurrencyCode.Equals(currRate.Key)))
                        {
                            _repository.CurrencyRates.Add(new Repository.DBModel.CurrencyRates() { 
                                Date = date.Key,
                                CurrencyCode = currRate.Key,
                                Amount = currRate.Value
                            });
                        }
                    }
                }

                _repository.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception("Error Saving new rates in DB", e);
            }

        }

    }
}

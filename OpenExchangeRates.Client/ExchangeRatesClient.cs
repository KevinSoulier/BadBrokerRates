using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ExchangeRates.Client;
using OpenExchangeRates.Client.Models;

namespace OpenExchangeRates.Client
{
    public class ExchangeRatesClient : IExchangeRatesClient
    {               
        public async Task<ExchangeRatesResponse> GetTimeSeries(DateTime Start, DateTime End, string Base, string Symbol)
        {
            
            string url = $"https://api.apilayer.com/exchangerates_data/timeseries?start_date={Start.ToString("yyyy-MM-dd")}&end_date={End.ToString("yyyy-MM-dd")}&base={Base}&symbol={Symbol}";

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", "OjtrvUyGwa5QDxsZ7u0BYnGYHWoxXlu6");
            
            HttpResponseMessage response = await client.GetAsync(url);
            ExchangeRatesResponse result = new ExchangeRatesResponse();
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    result = await response.Content.ReadFromJsonAsync<ExchangeRatesResponse>();
                }catch(Exception e)
                {
                    throw new Exception("Error Parsing response from Exchange Api", e);
                }
            }
            else
            {
                throw new Exception("Error Calling Exchange Api");
            }

          return result;
        }
    }
}

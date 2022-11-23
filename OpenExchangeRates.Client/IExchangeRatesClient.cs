using OpenExchangeRates.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Client
{
    public interface IExchangeRatesClient
    {
        Task<ExchangeRatesResponse> GetTimeSeries(DateTime Start, DateTime End, string Base, string Symbol);
    }
}

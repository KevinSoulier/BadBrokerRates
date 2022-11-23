using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenExchangeRates.Client.Models
{
    public class ExchangeRatesResponse
    {
        public string Base { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime EndDate { get; set; }
        public bool Success { get; set; }
        public bool Timeseries { get; set; }        
        public Dictionary<DateTime, Dictionary<string, decimal>> Rates { get; set; }

    }
}

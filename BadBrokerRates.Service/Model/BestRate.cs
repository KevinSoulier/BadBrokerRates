using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadBrokerRates.Service.Model
{
    public class BestRate
    {
        public Dictionary<DateTime, Dictionary<string, decimal>> Rates { get; set; }
        public DateTime BuyDate { get; set; }
        public DateTime SellDate { get; set; }
        public string Tool { get; set; }
        public decimal Revenue { get; set; }
    }
}

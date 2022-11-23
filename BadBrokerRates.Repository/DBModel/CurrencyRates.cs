using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadBrokerRates.Repository.DBModel
{
    [PrimaryKey(nameof(Date),nameof(CurrencyCode))]
    public class CurrencyRates
    {
        public DateTime Date { get; set; }        
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
    }
}

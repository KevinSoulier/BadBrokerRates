using BadBrokerRates.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadBrokerRates.Services
{
    public interface IRateService
    {
        Task<BestRate> GetBestRate(DateTime Start, DateTime End, decimal Amount);
    }
}

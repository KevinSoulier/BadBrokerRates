using BadBrokerRates.WebApi.Model;
using BadBrokerRates.Service.Model;
using BadBrokerRates.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BadBrokerRates.WebApi.Controllers
{
    [Route("api/rates")]
    [ApiController]
    public class RatesController : ControllerBase
    {
        IRateService _rateService;
        public RatesController(IRateService rateService)
        {
            _rateService= rateService;
        }

        [HttpGet(Name = "best")]
        public async Task<ActionResult<BestRateResponse>> GetBestRate([FromQuery(Name = "startDate")] DateTime StartDate, [FromQuery(Name = "endDate")] DateTime EndDate, [FromQuery(Name = "moneyUsd")] decimal MoneyUsd)
        {
            try
            {
                TimeSpan dif = EndDate - StartDate;
                if (dif.TotalDays >= 1 && dif.TotalDays <= 60)
                {
                    BestRateResponse response = new BestRateResponse();
                    BestRate bestRate = await _rateService.GetBestRate(StartDate, EndDate, MoneyUsd);

                    response.Rates = new List<Dictionary<string, object>>();

                    foreach (var rate in bestRate.Rates)
                    {
                        var r = new Dictionary<string, object>();
                        r.Add("date", rate.Key);
                        r.Add("rub", rate.Value["RUB"]);
                        r.Add("eur", rate.Value["EUR"]);
                        r.Add("gbp", rate.Value["GBP"]);
                        r.Add("jpy", rate.Value["JPY"]);
                        response.Rates.Add(r);
                    }

                    response.Revenue = bestRate.Revenue;
                    response.Tool = bestRate.Tool;
                    response.BuyDate = bestRate.BuyDate;
                    response.SellDate = bestRate.SellDate;

                    return Ok(response);
                }
                else
                {
                    return BadRequest("The Dates range can't be les than 2 days or greather than 60 days");
                }
            }catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}

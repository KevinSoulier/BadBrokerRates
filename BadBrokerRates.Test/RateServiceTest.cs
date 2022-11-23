using BadBrokerRates.Service;
using BadBrokerRates.Service;
using BadBrokerRates.Providers;
using Moq;
using Moq.Protected;
using BadBrokerRates.Providers.Models;

namespace BadBrokerRates.Test
{
    public class RateServiceTest
    {
        [Fact]
        public async Task RateService_GetBestRate()
        {
            Dictionary<DateTime, Dictionary<string, decimal>> data = new Dictionary<DateTime, Dictionary<string, decimal>>();
            data.Add(DateTime.Parse("2022-11-01"), new Dictionary<string, decimal>()
            {
                {"EUR", 1 },
                {"RUB", 1 },
                {"GBP", 1 },
                {"JPY", 1 }
            });
            data.Add(DateTime.Parse("2022-11-02"), new Dictionary<string, decimal>()
            {
                {"EUR", 1 },
                {"RUB", 3 },
                {"GBP", 1 },
                {"JPY", 1 }
            });
            data.Add(DateTime.Parse("2022-11-03"), new Dictionary<string, decimal>()
            {
                {"EUR", 1 },
                {"RUB", 1 },
                {"GBP", 1 },
                {"JPY", 1 }
            });
            data.Add(DateTime.Parse("2022-11-04"), new Dictionary<string, decimal>()
            {
                {"EUR", 1 },
                {"RUB", 1 },
                {"GBP", 1 },
                {"JPY", 5 }
            });
            data.Add(DateTime.Parse("2022-11-05"), new Dictionary<string, decimal>()
            {
                {"EUR", 1 },
                {"RUB", 1 },
                {"GBP", 1 },
                {"JPY", 1 }
            });
            data.Add(DateTime.Parse("2022-11-06"), new Dictionary<string, decimal>()
            {
                {"EUR", 1 },
                {"RUB", 1 },
                {"GBP", 1 },
                {"JPY", 1 }
            });


            var ratesProvider = new Mock<IRatesProvider>();
            ratesProvider.Setup(x => x.GetCurrencyRates(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(data);

            var service = new RateService(ratesProvider.Object);

            //Act
            var result = await service.GetBestRate(DateTime.Parse("2022-11-01"), DateTime.Parse("2022-11-06"), 100);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.BuyDate == DateTime.Parse("2022-11-04"));
            Assert.True(result.SellDate == DateTime.Parse("2022-11-05"));
            Assert.True(result.Tool == Currency.JPY.ToString());
        }
    }
}
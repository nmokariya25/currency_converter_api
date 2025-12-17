using CurrencyConverter.Infrastructure.Services;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Tests
{
    public class CurrencyServiceTests
    {
        [Fact]
        public async Task ConvertCurrencyAsync_ShouldThrow_WhenExcludedCurrency()
        {
            var httpClient = new HttpClient();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var service = new CurrencyService(httpClient, memoryCache);

            await Assert.ThrowsAsync<ArgumentException>(() => service.ConvertCurrencyAsync("EUR", "TRY", 100));
        }

        [Fact]
        public async Task GetLatestRatesAsync_ShouldReturnRates()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"base\":\"EUR\",\"rates\":{\"USD\":1.1,\"GBP\":0.9}}")
                });

            var client = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://api.frankfurter.app/")
            };
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var service = new CurrencyService(client, memoryCache);

            var rates = await service.GetLatestRatesAsync("EUR");

            rates.Should().ContainKey("USD");
            rates.Should().ContainKey("GBP");
        }
    }
}
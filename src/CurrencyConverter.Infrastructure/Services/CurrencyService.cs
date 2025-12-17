using CurrencyConverter.Application.Interfaces;
using CurrencyConverter.Application.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CurrencyConverter.Infrastructure.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private static readonly string[] ExcludedCurrencies = { "TRY", "PLN", "THB", "MXN" };

        public CurrencyService(
            HttpClient httpClient,
            IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<decimal> ConvertCurrencyAsync(string from, string to, decimal amount)
        {
            if (ExcludedCurrencies.Contains(from) || ExcludedCurrencies.Contains(to))
                throw new ArgumentException("Conversion not allowed for excluded currencies.");

            var response = await _httpClient.GetAsync($"/latest?from={from}&to={to}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<FrankfurterResponse>(json);

            return amount * data.Rates[to];
        }

        public async Task<Dictionary<string, decimal>> GetLatestRatesAsync(string baseCurrency)
        {
            string cacheKey = $"LatestRates_{baseCurrency}";
            if (_cache.TryGetValue(cacheKey, out Dictionary<string, decimal> cachedRates))
            {
                return cachedRates;
            }

            var response = await _httpClient.GetAsync($"/latest?from={baseCurrency}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<FrankfurterResponse>(json);

            var rates = data.Rates.Where(x => !ExcludedCurrencies.Contains(x.Key))
                                  .ToDictionary(x => x.Key, x => x.Value);

            // Cache for 5 minutes
            _cache.Set(cacheKey, rates, TimeSpan.FromMinutes(5));

            return rates;
        }

        public async Task<Dictionary<string, Dictionary<string, decimal>>> GetHistoricalRatesAsync(string baseCurrency, DateTime startDate, DateTime endDate, int page, int pageSize)
        {
            var response = await _httpClient.GetAsync($"/{startDate:yyyy-MM-dd}..{endDate:yyyy-MM-dd}?from={baseCurrency}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<FrankfurterHistoricalResponse>(json);

            var filtered = data.Rates.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Where(x => !ExcludedCurrencies.Contains(x.Key))
                                 .ToDictionary(x => x.Key, x => x.Value)
            );

            return filtered.Skip((page - 1) * pageSize)
                           .Take(pageSize)
                           .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}

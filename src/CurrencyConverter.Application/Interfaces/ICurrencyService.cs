using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Application.Interfaces
{
    public interface ICurrencyService
    {
        Task<decimal> ConvertCurrencyAsync(string from, string to, decimal amount);
        Task<Dictionary<string, decimal>> GetLatestRatesAsync(string baseCurrency);
        Task<Dictionary<string, Dictionary<string, decimal>>> GetHistoricalRatesAsync(string baseCurrency, DateTime startDate, DateTime endDate, int page, int pageSize);
    }
}

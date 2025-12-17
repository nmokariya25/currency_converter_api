using CurrencyConverter.Application.DTOs;
using CurrencyConverter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverter.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,User")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public ExchangeRatesController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestRates([FromQuery] string baseCurrency = "EUR")
        {
            var rates = await _currencyService.GetLatestRatesAsync(baseCurrency);
            return Ok(rates);
        }

        [HttpPost("convert")]
        public async Task<IActionResult> ConvertCurrency([FromBody] ConversionRequestDto request)
        {
            try
            {
                var result = await _currencyService.ConvertCurrencyAsync(request.From, request.To, request.Amount);
                return Ok(new ConversionResponseDto
                {
                    From = request.From,
                    To = request.To,
                    OriginalAmount = request.Amount,
                    ConvertedAmount = result,
                    Date = DateTime.UtcNow
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("historical")]
        public async Task<IActionResult> GetHistoricalRates([FromQuery] string baseCurrency, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var data = await _currencyService.GetHistoricalRatesAsync(baseCurrency, startDate, endDate, page, pageSize);
            return Ok(data);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyConversion.ConversionRepository;
using CurrencyConversion.Money;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConversion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyConversionController : ControllerBase
    {
        private readonly ICurrencyConversionRepository _repo;

        public CurrencyConversionController(ICurrencyConversionRepository repo) =>
            _repo = repo;
    
        /// <summary>
        /// Converts from one currency to another using the latest known conversion rate.
        /// </summary>
        /// <param name="fromCurrency">Currency code to convert from</param>
        /// <param name="toCurrency">Currency code to convert to</param>
        /// <param name="amount">Amount of the <see cref="fromCurrency"/> to convert into <see cref="toCurrency"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CurrencyConversionViewModel> Convert(string fromCurrency, string toCurrency, decimal amount)
        {
            fromCurrency = fromCurrency.Trim().ToUpperInvariant();
            toCurrency = toCurrency.Trim().ToUpperInvariant();

            var givenUsdConversion = await _repo.GetUsConversionByCountryCodeAsync(fromCurrency);
            var requestedUsdConversion = await _repo.GetUsConversionByCountryCodeAsync(toCurrency);

            if (givenUsdConversion == null)
            {
                throw new BadHttpRequestException($"The provided currency {fromCurrency} does not have a known US conversion rate.");

            }
            if (requestedUsdConversion == null)
            {
                throw new BadHttpRequestException($"The provided currency {toCurrency} does not have a known US conversion rate.");
            }

            var fromAmount = new Money.Money(new Currency(fromCurrency), amount);
            var usdAmount = givenUsdConversion.Value.Convert(fromAmount);
            var toAmount = requestedUsdConversion.Value.Convert(usdAmount);

            return new CurrencyConversionViewModel
            {
                FromCurrency = givenUsdConversion.Value.BaseCurrency, 
                Amount = toAmount
            };
        }
    }

    public class CurrencyConversionViewModel
    {
        public Currency FromCurrency { get; set; }
        public Money.Money Amount { get; set; }
    }
}

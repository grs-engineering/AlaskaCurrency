namespace CurrencyConversion.Money;

public readonly record struct MoneyRate
{
    /// <summary>
    /// This is the currency the rate will normally convert TO
    /// </summary>
    public Currency TargetCurrency { get; }
    
    /// <summary>
    /// This is the currency that the rate is based on and will be converted FROM
    /// </summary>
    public Currency BaseCurrency { get; }
    
    /// <summary>
    /// This is the ratio of <see cref="TargetCurrency"/> / <see cref="BaseCurrency"/>
    /// </summary>
    public decimal ConversionRate { get; }
    
    /// <summary>
    /// Generates a new conversion rate from the base to the target
    /// </summary>
    /// <param name="targetCurrency">This is the currency the rate will normally convert TO</param>
    /// <param name="baseCurrency">This is the currency that the rate is based on and will be converted FROM</param>
    /// <param name="conversionRate">This is the ratio of <see cref="targetCurrency"/> / <see cref="baseCurrency"/></param>
    public MoneyRate(Currency targetCurrency, Currency baseCurrency, decimal conversionRate) 
    {
        // Ensure that if we got the same currencies, we ALWAYS convert the same way
        ConversionRate = targetCurrency == baseCurrency ?
            1 :
            conversionRate;
        
        TargetCurrency = targetCurrency;
        BaseCurrency = baseCurrency;

        if (conversionRate <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(conversionRate), conversionRate,
                "Conversion rate cannot be 0 or negative.");
        }
    }

    /// <summary>
    /// Converts an amount of money between the currencies given the exchange rate
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Money Convert(Money amount)
    {
        // ConversionRate = Target / Base
        
        // Normal conversion from the base to the target
        if (amount.Currency == BaseCurrency)
        {
            // TargetAmt = Conversion * BaseAmt
            return new Money(TargetCurrency, amount.Value * ConversionRate);
        }
        
        // Converting back from the target to the base
        if (amount.Currency == TargetCurrency)
        {
            // Base = Target / Conversion
            return new Money(BaseCurrency, amount.Value / ConversionRate);
        }

        throw new ArgumentOutOfRangeException(nameof(amount),
            $"Currency {amount.Currency} does not apply to this rate");
    }
}
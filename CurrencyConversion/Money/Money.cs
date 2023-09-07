namespace CurrencyConversion.Money;

/// <summary>
/// Represents a specific amount of money in a given currency.
/// </summary>
public readonly record struct Money
{
    public Currency Currency { get; }
    public decimal Value { get; init; } = decimal.Zero;
    
    public Money(Currency currency, decimal value)
    {
        Currency = currency;
        Value = value;
    }
    public Money(Currency currency)
    {
        Currency = currency;
    }

    public static Money operator -(Money a) => new (a.Currency, -1 * a.Value);
    public static Money operator -(Money a, Money b) => a + (-b);
    public static Money operator +(Money a, Money b)
    {
        if (!a.Currency.Equals(b.Currency))
        {
            throw new ArgumentException(
                $"Cannot combine {a.Currency.CurrencyName} ({a.Currency.CountryCode}) with {b.Currency.CurrencyName} ({b.Currency.CountryCode}).");
        }

        return new Money(a.Currency, a.Value + b.Value);
    }
}
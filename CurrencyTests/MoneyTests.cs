using CurrencyConversion;
using CurrencyConversion.Money;

namespace CurrencyTests;

public class MoneyTests
{
    [Fact]
    public void EqualityTest() =>
        Assert.StrictEqual(
            new Money(Currency.CAD, 0), 
            new Money(Currency.CAD, 0));

    [Fact]
    public void ValueInequality() =>
        Assert.NotStrictEqual(
            new Money(Currency.CAD, 0),
            new Money(Currency.CAD, 1));

    [Fact]
    public void CurrencyInequality() =>
        Assert.NotStrictEqual(
            new Money(Currency.CAD, 1),
            new Money(Currency.USD, 1));

    [Fact]
    public void Add() =>
        Assert.StrictEqual(
            new Money(Currency.CAD, 3),
            new Money(Currency.CAD, 1) + new Money(Currency.CAD, 2));
    
    [Fact]
    public void Subtract() =>
        Assert.StrictEqual(
            new Money(Currency.CAD, -1),
            new Money(Currency.CAD, 1) - new Money(Currency.CAD, 2));

    [Fact]
    public void Money_CannotCombine_DissimilarCurrencies()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var x = new Money(Currency.GBP, 1) + new Money(Currency.USD, 1);
        });
        
        Assert.Throws<ArgumentException>(() =>
        {
            var x = new Money(Currency.GBP, 1) - new Money(Currency.USD, 1);
        });
    }
}
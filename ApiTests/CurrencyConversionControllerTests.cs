using CurrencyConversion.Controllers;
using CurrencyConversion.ConversionRepository;
using CurrencyConversion.Money;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ApiTests;

public class CurrencyConversionControllerTests
{
    [Fact]
    public async Task Convert_Converts_SameCurrency()
    {
        // Arrange
        var mockRepo = new Mock<ICurrencyConversionRepository>();
        mockRepo.Setup(repo =>
                repo.GetUsConversionByCountryCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(new MoneyRate(Currency.USD, Currency.USD, 1));
        
        var controller = new CurrencyConversionController(mockRepo.Object);
        
        // Act
        var result = await controller.Convert("USD", "USD", 2);

        // Assert
        var model = Assert.IsType<CurrencyConversionViewModel>(result);
        
        Assert.Equal("USD", model.FromCurrency.CountryCode);
        Assert.Equal("USD", model.Amount.Currency.CountryCode);
        Assert.Equal(2, model.Amount.Value);
    }

    [Fact]
    public async Task Convert_Converts_WithKnownCurrencies()
    {
        // Arrange
        var mockRepo = new Mock<ICurrencyConversionRepository>();
        mockRepo.Setup(repo =>
                repo.GetUsConversionByCountryCodeAsync(It.Is<string>("USD", StringComparer.InvariantCulture)))
            .ReturnsAsync(new MoneyRate(Currency.USD, Currency.USD, 1));
        mockRepo.Setup(repo =>
                repo.GetUsConversionByCountryCodeAsync(It.Is<string>("CAD", StringComparer.InvariantCulture)))
            .ReturnsAsync(new MoneyRate(Currency.USD, Currency.CAD, new decimal(1.5)));
        
        var controller = new CurrencyConversionController(mockRepo.Object);
        
        // Act
        var result = await controller.Convert("CAD", "USD", 1);

        // Assert
        var model = Assert.IsType<CurrencyConversionViewModel>(result);
        
        Assert.Equal("CAD", model.FromCurrency.CountryCode);
        Assert.Equal("USD", model.Amount.Currency.CountryCode);
        Assert.Equal(new decimal(1.5), model.Amount.Value);
    }

    [Fact]
    public async Task Convert_Throws_WithUnknownCurrencies()
    {
        // Arrange
        var mockRepo = new Mock<ICurrencyConversionRepository>();
        mockRepo.Setup(repo =>
                repo.GetUsConversionByCountryCodeAsync(It.Is<string>("USD", StringComparer.InvariantCulture)))
            .ReturnsAsync((MoneyRate?)null);
        var controller = new CurrencyConversionController(mockRepo.Object);
        
        // Act
        var ex =
            await Assert.ThrowsAsync<BadHttpRequestException>(async() => 
                await controller.Convert("CAD", "USD", 1));
    }
}
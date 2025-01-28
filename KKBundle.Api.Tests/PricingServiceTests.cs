using KKBundle.API.Domains.Pricing;
using KKBundle.API.Domains.Providers;
using Microsoft.Extensions.Options;
using Moq;

namespace KKBundle.Api.Tests;

public class PricingServiceTests
{
    private readonly IOptions<ProviderTopicsOptions> providerTopicsOptions;
    private Mock<IProviderFactory> mockProviderFactory;

    public PricingServiceTests()
    {
        providerTopicsOptions = Options.Create(new ProviderTopicsOptions
        {
            ProviderTopics = new Dictionary<string, string>
            {
                { "provider_a", "math+science" },
                { "provider_b", "reading+science" }
            }
        });

        mockProviderFactory = new Mock<IProviderFactory>();
        mockProviderFactory
            .Setup(factory => factory.Create(It.IsAny<KeyValuePair<string, string>>()))
            .Returns((KeyValuePair<string, string> kvp) =>
            {
                var provider = new Provider(kvp.Key, kvp.Value.Split("+"));
                return provider;
            });
    }

    [Fact]
    public void When_CalculateOffer_ReturnsExpectedPricingList()
    {
        // Arrange
        var pricingService = new PricingService(mockProviderFactory.Object, providerTopicsOptions);

        var pricingQuery = new PricingQuery(new Dictionary<string, int>
        {
            { "math", 50 },
            { "science", 30 },
            { "reading", 20 },
            { "history", 10 } 
        });

        // Act
        var result = pricingService.CalculateOffer(pricingQuery);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(8, result.Single(x => x.ProviderName == "provider_a").Amount);
        Assert.Equal(5, result.Single(x => x.ProviderName == "provider_b").Amount);
    }

    [Fact]
    public void When_CalculateOffer_QueryDoesNotMatchProviders_Returns_Zero()
    {
        // Arrange
        var pricingService = new PricingService(mockProviderFactory.Object, providerTopicsOptions);

        var pricingQuery = new PricingQuery(new Dictionary<string, int>
        {
            { "art", 50 },
            { "history", 10 }
        });

        // Act
        var result = pricingService.CalculateOffer(pricingQuery);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, pricing => Assert.Equal(0, pricing.Amount));
    }
}
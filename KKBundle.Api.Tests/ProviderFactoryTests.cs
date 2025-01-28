using KKBundle.API.Domains.Providers;

namespace KKBundle.Api.Tests;

public class ProviderFactoryTests
{
    [Fact]
    public void Create_ReturnsProvider_WhenValidInputProvided()
    {
        // Arrange
        var factory = new ProviderFactory();
        var providerDetails = new KeyValuePair<string, string>("p1", "m+s");

        // Act
        var provider = factory.Create(providerDetails);

        // Assert
        Assert.NotNull(provider);
    }

    [Fact]
    public void Create_ThrowsArgumentNullException_WhenProviderNameIsNull()
    {
        // Arrange
        var factory = new ProviderFactory();
        var providerDetails = new KeyValuePair<string, string>(null, "math+science");

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => factory.Create(providerDetails));
        Assert.Equal("provider.name", exception.ParamName);
    }

    [Fact]
    public void Create_ThrowsArgumentNullException_WhenProviderTopicsIsNull()
    {
        // Arrange
        var factory = new ProviderFactory();
        var providerDetails = new KeyValuePair<string, string>("provider_a", null);

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => factory.Create(providerDetails));
        Assert.Equal("provider.topics", exception.ParamName);
    }
}
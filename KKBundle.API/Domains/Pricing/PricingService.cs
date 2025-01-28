using KKBundle.API.Domains.Providers;
using Microsoft.Extensions.Options;

namespace KKBundle.API.Domains.Pricing;

public interface IPricingService
{
    List<Pricing> CalculateOffer(PricingQuery query);
}

public class PricingService : IPricingService
{
    private readonly List<Provider> providers = [];

    public PricingService(IProviderFactory providerFactory, IOptions<ProviderTopicsOptions> providerTopicOptions)
    {
        providers = [];

        foreach (var providerTopic in providerTopicOptions.Value.ProviderTopics)
        {
            providers.Add(providerFactory.Create(providerTopic));
        }
    }

    public List<Pricing> CalculateOffer(PricingQuery query)
    {
        var pricingList = new List<Pricing>();
        var topTopics = query.Topics
            .OrderByDescending(t => t.Value)
            .Take(3)
            .ToDictionary(t => t.Key, t => t.Value);

        foreach (var provider in providers)
        {
            pricingList.Add(provider.Calculate(topTopics));
        }

        return pricingList;
    }
}
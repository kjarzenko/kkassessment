namespace KKBundle.API.Domains.Providers;

public interface IProviderFactory
{
    Provider Create(KeyValuePair<string, string> providerDetails);
}

public class ProviderFactory : IProviderFactory
{
    public Provider Create(KeyValuePair<string, string> providerDetails)
    {
        if (string.IsNullOrWhiteSpace(providerDetails.Key))
            throw new ArgumentNullException("provider.name");

        if (string.IsNullOrWhiteSpace(providerDetails.Value))
            throw new ArgumentNullException("provider.topics");

        var providerTopics = providerDetails.Value.Split("+");
        return new Provider(providerDetails.Key, providerTopics);
    }
}
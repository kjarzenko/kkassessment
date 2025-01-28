namespace KKBundle.API.Domains.Providers;

public class Provider(string name, string[] topics)
{
    public Pricing.Pricing Calculate(Dictionary<string, int> queryTopics)
    {
        var matchingTopics = topics.Intersect(queryTopics.Keys).ToList();

        if (matchingTopics.Count is 2)
            return new Pricing.Pricing(name, 0.1m * matchingTopics.Sum(topic => queryTopics[topic]));

        var topic = matchingTopics.SingleOrDefault();
        
        if (topic is null)
            return new Pricing.Pricing(name, 0);

        var rank = queryTopics.Keys.ToList().IndexOf(topic!);

        decimal quoteAmount = rank switch
        {
            0 => 0.2m * queryTopics[topic],
            1 => 0.25m * queryTopics[topic],
            2 => 0.3m * queryTopics[topic],
            _ => 0
        };

        return new Pricing.Pricing(name, quoteAmount);
    }
}
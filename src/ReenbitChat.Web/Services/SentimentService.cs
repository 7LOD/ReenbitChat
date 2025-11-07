using Azure;
using Azure.AI.TextAnalytics;
using ReenbitChat.Domain;

namespace ReenbitChat.Web.Services
{
    public class SentimentService
    {
        private readonly TextAnalyticsClient _client;

        public SentimentService(IConfiguration config)
        {
            var endpoint = config["AzureCognitive:Endpoint"];
            var key = config["AzureCognitive:Key"];
            _client = new TextAnalyticsClient(new Uri(endpoint), new AzureKeyCredential(key));
        }

        public async Task<Sentiment> AnalizyAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Sentiment.None;
            }
            var result = await _client.AnalyzeSentimentAsync(text);
            return result.Value.Sentiment switch
            {
                TextSentiment.Positive => Sentiment.Positive,
                TextSentiment.Neutral => Sentiment.Neutral,
                TextSentiment.Negative => Sentiment.Negative,
                _ => Sentiment.None
            };
        }
    }
}

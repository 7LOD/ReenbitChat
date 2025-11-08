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

            var options = new TextAnalyticsClientOptions
            {
                Retry =
        {
            Mode = Azure.Core.RetryMode.Fixed,
            Delay = TimeSpan.FromSeconds(1),
            MaxRetries = 2,
            NetworkTimeout = TimeSpan.FromSeconds(3)
        }
            };

            _client = new TextAnalyticsClient(new Uri(endpoint), new AzureKeyCredential(key), options);
        }

        public async Task<Sentiment> AnalizyAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return Sentiment.None;

            try
            {
                var response = await _client.AnalyzeSentimentAsync(text);

                var sentiment = response.Value.Sentiment;
                return sentiment switch
                {
                    TextSentiment.Positive => Sentiment.Positive,
                    TextSentiment.Neutral => Sentiment.Neutral,
                    TextSentiment.Negative => Sentiment.Negative,
                    _ => Sentiment.None
                };
            }
            catch (RequestFailedException ex)
            {
                return Sentiment.None;
            }
            catch (Exception ex)
            {
                return Sentiment.None;
            }
        }
    }
}

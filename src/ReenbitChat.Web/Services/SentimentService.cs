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

            if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(key))
                throw new InvalidOperationException("Azure Cognitive credentials are missing. Check AzureCognitive:Endpoint and AzureCognitive:Key in configuration.");

            _client = new TextAnalyticsClient(new Uri(endpoint), new AzureKeyCredential(key));
        }

        public async Task<Sentiment> AnalizyAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return Sentiment.None;

            try
            {
                var response = await _client.AnalyzeSentimentAsync(text);
                var sentiment = response.Value.Sentiment;
                Console.WriteLine($"[SentimentService] '{text}' => {response.Value.Sentiment}");
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
                Console.WriteLine($"[SentimentService] Azure error: {ex.Message}");
                return Sentiment.None;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SentimentService] General error: {ex.Message}");
                return Sentiment.None;
            }
        }
    }
}

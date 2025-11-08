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
                Console.WriteLine($"[SentimentService] Calling API: {text}");
                var response = await _client.AnalyzeSentimentAsync(text);
                Console.WriteLine($"[SentimentService] Completed for '{text}'");

                var sentiment = response.Value.Sentiment;
                
                Console.WriteLine($"[SentimentService] '{text}' => {response.Value.Sentiment}");
                return response.Value.Sentiment switch
                {
                    TextSentiment.Positive => Sentiment.Positive, 
                    TextSentiment.Neutral => Sentiment.Neutral,   
                    TextSentiment.Negative => Sentiment.Negative, 
                    TextSentiment.Mixed => Sentiment.Neutral,     
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

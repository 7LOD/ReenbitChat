using Azure;
using Azure.AI.TextAnalytics;
using ReenbitChat.Domain;

namespace ReenbitChat.Web.Services
{
    public class SentimentService
    {
        private readonly TextAnalyticsClient _client;
        private readonly ILogger<SentimentService> _log;

        public SentimentService(IConfiguration config, ILogger<SentimentService> log)
        {
            _log = log;
            var endpoint = config["AzureCognitive:Endpoint"];
            var key = config["AzureCognitive:Key"];

            _log.LogInformation("Cognitive cfg: endpoint set={EndpointSet}, key set={KeySet}",
                !string.IsNullOrWhiteSpace(endpoint),
                !string.IsNullOrWhiteSpace(key));

            if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(key))
                throw new InvalidOperationException("Azure Cognitive credentials are missing. Check AzureCognitive:Endpoint and AzureCognitive:Key in configuration.");

            _client = new TextAnalyticsClient(new Uri(endpoint), new AzureKeyCredential(key));
        }

        public async Task<int> AnalizyAsync(string text)
        {
            try
            {
                _log.LogInformation("Analyzing text len={Len}", text?.Length ?? 0);
                var res = await _client.AnalyzeSentimentAsync(text);
                var score = res.Value.Sentiment switch
                {
                    Azure.AI.TextAnalytics.TextSentiment.Positive => 1,
                    Azure.AI.TextAnalytics.TextSentiment.Negative => -1,
                    _ => 0
                };
                _log.LogInformation("Analysis ok score={Score}", score);
                return score;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Sentiment analysis failed");
                return 0;
            }
        }
    }

}

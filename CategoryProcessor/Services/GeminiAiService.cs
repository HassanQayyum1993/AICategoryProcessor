using CategoryProcessor.Abstractions;
using CategoryProcessor.Factories;
using CategoryProcessor.Models;
using Newtonsoft.Json;
using RestSharp;

namespace CategoryProcessor.Services
{
    public class GeminiAiService : IAiService
    {
        private readonly RestClient _client;
        private readonly string _apiKey;
        private readonly ILogger<GeminiAiService> _logger;

        public GeminiAiService(IConfiguration configuration, ILogger<GeminiAiService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var baseUrl = configuration["GeminiAi:Url"];
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException("API URL is not configured.");

            _client = new RestClient(baseUrl);
            _apiKey = configuration["GeminiAi:ApiKey"];
            if (string.IsNullOrWhiteSpace(_apiKey))
                throw new ArgumentException("API Key is not configured.");
        }

        public async Task<List<string>> GenerateContentAsync(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                throw new ArgumentException("Prompt cannot be null or empty.", nameof(prompt));

            var request = new RestRequest("", Method.Post);
            request.AddHeader("x-goog-api-key", _apiKey);

            var requestBody = GeminiRequestFactory.CreateRequest(prompt);
            request.AddJsonBody(requestBody);

            try
            {
                var response = await _client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    // Ensure response content is not null or empty
                    if (string.IsNullOrWhiteSpace(response.Content))
                    {
                        _logger.LogWarning("Response content is null or empty.");
                        throw new InvalidOperationException("Response content is null or empty.");
                    }

                    // Deserialize the response
                    var content = JsonConvert.DeserializeObject<GeminiResponse>(response.Content);

                    if (content?.Candidates == null || content.Candidates.Count() == 0)
                    {
                        _logger.LogWarning("No candidates found in the response.");
                        throw new InvalidOperationException("No candidates found in the response.");
                    }

                    var text = content.Candidates[0]?.Content?.Parts?[0]?.Text;

                    if (string.IsNullOrWhiteSpace(text))
                    {
                        _logger.LogWarning("Response text is null or empty.");
                        throw new InvalidOperationException("Response text is null or empty.");
                    }

                    // Split the text and return the result
                    var attributes = text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    return new List<string>(attributes);
                }
                else
                {
                    _logger.LogError("Request failed with status code: {StatusCode} and message: {Content}",
                        response.StatusCode, response.Content);
                    throw new InvalidOperationException($"Request failed with status code: {response.StatusCode} and message: {response.Content}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error occurred while generating content.");
                return new List<string>();
            }
        }
    }
}

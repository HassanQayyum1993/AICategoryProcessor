using CategoryProcessor.Abstractions;
using CategoryProcessor.Factories;
using CategoryProcessor.Models;
using Newtonsoft.Json;
using RestSharp;

namespace CategoryProcessor.Services
{
    public class GeminiAiService: IAiService
    {
        private readonly RestClient _client;
        private readonly string _apiKey;

        public GeminiAiService(IConfiguration configuration)
        {
            _client = new RestClient(configuration["GeminiAi:Url"]);
            _apiKey = configuration["GeminiAi:ApiKey"];
        }

        public async Task<List<string>> GenerateContentAsync(string prompt)
        {
            var request = new RestRequest("", Method.Post);
            request.AddHeader("x-goog-api-key", $"{_apiKey}");

            var requestBody = GeminiRequestFactory.CreateRequest(prompt);
            request.AddJsonBody(requestBody);

            var response = await _client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                var content = JsonConvert.DeserializeObject<GeminiResponse>(response.Content);
                var text = content?.Candidates[0].Content.Parts[0].Text;
                var attributes = text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                return new List<string>(attributes);
            }

            return new List<string>();
        }
    }

}

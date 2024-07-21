using RestSharp;
using Newtonsoft.Json.Linq;
using CategoryProcessor.Abstractions;

namespace CategoryProcessor.Services
{
    // Services/OpenAiService.cs


    public class OpenAiService: IAiService
    {
        private readonly string _apiKey;
        private readonly RestClient _client;

        public OpenAiService(IConfiguration configuration)
        {
            _apiKey = configuration["OpenAi:ApiKey"];
            //_client = new RestClient("https://api.openai.com/v1/completions");
            _client = new RestClient(configuration["OpenAi:Url"]);
        }

        public async Task<List<string>> GenerateContentAsync(string prompt)
        {
            var modelName = "gpt-4o-mini";

            var request = new RestRequest("", Method.Post);
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                model = modelName,
                prompt = prompt,
                max_tokens = 50
            });

            var response = await _client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                var content = JObject.Parse(response.Content);
                var text = content["choices"][0]["text"].ToString();
                var attributes = text.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                return new List<string>(attributes);
            }

            return new List<string>();
        }
    }
}

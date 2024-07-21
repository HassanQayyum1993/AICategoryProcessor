namespace CategoryProcessor.Models
{
    public class GeminiRequest
    {
        public GeminiContent[] Contents { get; set; }
        public GenerationConfig GenerationConfig { get; set; }
        public SafetySetting[] SafetySettings { get; set; }
    }

    public class GeminiContent
    {
        public string Role { get; set; }
        public GeminiPart[] Parts { get; set; }
    }

    public class GeminiPart
    {
        public string Text { get; set; }
    }

    public class GenerationConfig
    {
        public int Temperature { get; set; }
        public int TopK { get; set; }
        public int TopP { get; set; }
        public int MaxOutputTokens { get; set; }
        public List<object> StopSequences { get; set; }
    }

    public class SafetySetting
    {
        public string Category { get; set; }
        public string Threshold { get; set; }
    }
}

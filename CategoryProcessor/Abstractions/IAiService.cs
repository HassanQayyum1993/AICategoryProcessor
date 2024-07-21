namespace CategoryProcessor.Abstractions
{
    public interface IAiService
    {
        Task<List<string>> GenerateContentAsync(string categoryName);
    }
}

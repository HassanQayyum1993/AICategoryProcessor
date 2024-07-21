using CategoryProcessor.Abstractions;
using CategoryProcessor.Models;
using Microsoft.AspNetCore.Mvc;

namespace CategoryProcessor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IAiService _aiService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(IAiService aiService, ILogger<CategoryController> logger)
        {
            _aiService = aiService ?? throw new ArgumentNullException(nameof(aiService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> ProcessCategories([FromBody] List<Category> categories)
        {
            if (categories == null || categories.Count == 0)
            {
                _logger.LogWarning("Received an empty or null categories list.");
                return BadRequest("Categories list cannot be null or empty.");
            }

            string prompt;
            var result = new List<CategoryAttributes>();

            foreach (var category in categories)
            {
                foreach (var subCategory in category.SubCategories)
                {
                    prompt = $"List the three most popular attributes for the category: {subCategory.CategoryName}.";

                    try
                    {
                        var attributes = await _aiService.GenerateContentAsync(prompt);
                        result.Add(new CategoryAttributes
                        {
                            CategoryId = subCategory.CategoryId,
                            Attributes = attributes
                        });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error occurred while generating content for category: {CategoryName}", subCategory.CategoryName);
                        // Optionally add error details to the result or continue
                        result.Add(new CategoryAttributes
                        {
                            CategoryId = subCategory.CategoryId,
                            Attributes = new List<string> { "Error occurred while processing attributes." }
                        });
                    }
                }
            }

            return Ok(result);
        }
    }
}

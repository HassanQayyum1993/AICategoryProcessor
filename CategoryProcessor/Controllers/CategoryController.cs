using CategoryProcessor.Abstractions;
using CategoryProcessor.Models;
using Microsoft.AspNetCore.Mvc;

namespace CategoryProcessor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IAiService _IAiService;
        public CategoryController(IAiService IAiService)
        {
            _IAiService = IAiService;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessCategories([FromBody] List<Category> categories)
        {
            string prompt = String.Empty;
            var result = new List<CategoryAttributes>();

            foreach (var category in categories)
            {
                foreach (var subCategory in category.SubCategories)
                {
                    prompt = $"List the three most popular attributes for the category: {subCategory.CategoryName}.";
                    var attributes = await _IAiService.GenerateContentAsync(prompt);
                    result.Add(new CategoryAttributes
                    {
                        CategoryId = subCategory.CategoryId,
                        Attributes = attributes
                    });
                }
            }
            return Ok(result);
        }
    }
}

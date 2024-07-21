using CategoryProcessor.Controllers;
using CategoryProcessor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging; // Ensure you have this namespace
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CategoryProcessor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly CategoryController _categoryController;

        public IndexModel(ILogger<IndexModel> logger, CategoryController categoryController)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _categoryController = categoryController ?? throw new ArgumentNullException(nameof(categoryController));
        }

        [BindProperty]
        public string CategoryJson { get; set; }

        public string OutputJson { get; set; }

        public async Task<IActionResult> OnPostSubmit()
        {
            if (CategoryJson is null)
            {
                return BadRequest(ModelState);
            }
            List<Category> categories;
            try
            {
                categories = JsonConvert.DeserializeObject<List<Category>>(CategoryJson);

                if (categories == null || categories.Count == 0)
                {
                    _logger.LogWarning("Deserialized categories list is null or empty.");
                    OutputJson = "Categories list cannot be null or empty.";
                    return Page();
                }

                var result = await _categoryController.ProcessCategories(categories) as OkObjectResult;
                if (result != null)
                {
                    OutputJson = JsonConvert.SerializeObject(result.Value, Formatting.Indented);
                }
                else
                {
                    _logger.LogWarning("The result from ProcessCategories was not an OkObjectResult.");
                    OutputJson = "Error processing categories.";
                }
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Error deserializing CategoryJson.");
                OutputJson = "Error deserializing categories. Please ensure the JSON format is correct.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred during category processing.");
                OutputJson = "An unexpected error occurred. Please try again.";
            }

            return Page();
        }
    }
}

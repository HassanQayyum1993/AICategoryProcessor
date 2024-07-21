using CategoryProcessor.Controllers;
using CategoryProcessor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using RestSharp;

namespace CategoryProcessor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly CategoryController _categoryController;

        public IndexModel(ILogger<IndexModel> logger, CategoryController categoryController)
        {
            _logger = logger;
            _categoryController = categoryController;
        }

        [BindProperty]
        public string CategoryJson { get; set; }
        public string OutputJson { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostSubmit()
        {
            var categories = JsonConvert.DeserializeObject<List<Category>>(CategoryJson);

            var result = await _categoryController.ProcessCategories(categories) as OkObjectResult;
            if (result != null)
            {
                OutputJson = JsonConvert.SerializeObject(result.Value, Formatting.Indented);
            }

            return Page();
        }
    }
}

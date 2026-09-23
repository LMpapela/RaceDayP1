using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDayP1.Models;
using RaceDayP1.Services;
using System.Security.Claims;

// Beginning of Categories controller namespace
namespace RaceDayP1.Controller
{
    // Beginning of CategoriesController class
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        // Beginning of CategoriesController constructor
        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        // End of CategoriesController constructor

        // Beginning of GetCategories method
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategories();

            return Ok(categories);
        }
        // End of GetCategories method

        // Beginning of GetCategory method
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category =
                await _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }
        // End of GetCategory method

        // Beginning of CreateCategory method
        [Authorize(Roles = "Organiser")]
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(
            Category category)
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return Unauthorized();
            }

            var result = await _categoryService.CreateCategory(category,currentUserId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(
                nameof(GetCategory),
                new { id = result.Category!.CategoryID },
                result.Category);
        }
        // End of CreateCategory method

        // Beginning of UpdateCategory method
        [Authorize(Roles = "Organiser")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            if (id != category.CategoryID)
            {
                return BadRequest();
            }

            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return Unauthorized();
            }

            var result =
                await _categoryService.UpdateCategory(category, currentUserId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }
        // End of UpdateCategory method

        // Beginning of DeleteCategory method
        [Authorize(Roles = "Organiser")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return Unauthorized();
            }

            var result =
                await _categoryService.DeleteCategory(
                    id,
                    currentUserId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }
        // End of DeleteCategory method
    }
    // End of CategoriesController class
}
// End of Categories controller namespace
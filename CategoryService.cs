
using Microsoft.EntityFrameworkCore;
using RaceDayP1.Data;
using RaceDayP1.Models;

// Beginning of Category service namespace
namespace RaceDayP1.Services
{
    // Beginning of CategoryService class
    public class CategoryService
    {
        private readonly RaceDayDbContext _context;

        // Beginning of CategoryService constructor
        public CategoryService(RaceDayDbContext context)
        {
            _context = context;
        }
        // End of CategoryService constructor

        // Beginning of GetAllCategories method
        public async Task<List<Category>> GetAllCategories()
        {
            return await _context.Categories
                .AsNoTracking()
                .ToListAsync();
        }
        // End of GetAllCategories method

        // Beginning of GetCategoryById method
        public async Task<Category?> GetCategoryById(int id)
        {
            return await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryID == id);
        }
        // End of GetCategoryById method

        // Beginning of CreateCategory method
        public async Task<(bool Success, string Message, Category? Category)>
            CreateCategory(Category category, int organiserId)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                return (false, "Category name is required.", null);
            }

            if (string.IsNullOrWhiteSpace(category.Description))
            {
                return (false, "Category description is required.", null);
            }

            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == category.EventID);

            if (eventItem == null)
            {
                return (false, "The selected event does not exist.", null);
            }

            if (eventItem.OrganiserID != organiserId)
            {
                return (
                    false,
                    "You can only create categories for your own events.",
                    null);
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return (true, "Category created successfully.", category);
        }
        // End of CreateCategory method

        // Beginning of UpdateCategory method
        public async Task<(bool Success, string Message)> UpdateCategory(
            Category category,
            int organiserId)
        {
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryID == category.CategoryID);

            if (existingCategory == null)
            {
                return (false, "The category does not exist.");
            }

            var existingEvent = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == existingCategory.EventID);

            if (existingEvent == null)
            {
                return (false, "The event associated with this category does not exist.");
            }

            if (existingEvent.OrganiserID != organiserId)
            {
                return (false, "You can only update categories for your own events.");
            }

            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                return (false, "Category name is required.");
            }

            if (string.IsNullOrWhiteSpace(category.Description))
            {
                return (false, "Category description is required.");
            }

            // Only allow moving a category to an event owned by this organiser
            var newEvent = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == category.EventID);

            if (newEvent == null)
            {
                return (false, "The selected event does not exist.");
            }

            if (newEvent.OrganiserID != organiserId)
            {
                return (false, "You can only move categories to your own events.");
            }

            existingCategory.EventID = category.EventID;
            existingCategory.CategoryName = category.CategoryName.Trim();
            existingCategory.Description = category.Description.Trim();

            await _context.SaveChangesAsync();

            return (true, "Category updated successfully.");
        }
        // End of UpdateCategory method

        // Beginning of DeleteCategory method
        public async Task<(bool Success, string Message)> DeleteCategory(
            int id,
            int organiserId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryID == id);

            if (category == null)
            {
                return (false, "The category does not exist.");
            }

            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == category.EventID);

            if (eventItem == null)
            {
                return (false, "The event associated with this category does not exist.");
            }

            if (eventItem.OrganiserID != organiserId)
            {
                return (false, "You can only delete categories for your own events.");
            }

            // Preserve enrolments that refer to this category
            bool hasEnrolments = await _context.Enrolments
                .AnyAsync(e => e.CategoryID == id);

            if (hasEnrolments)
            {
                return (
                    false,
                    "This category cannot be deleted because participants are enrolled in it.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return (true, "Category deleted successfully.");
        }
        // End of DeleteCategory method
    }
    // End of CategoryService class
}
// End of Category service namespace

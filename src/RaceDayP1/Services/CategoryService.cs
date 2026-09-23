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
            return await _context.Categories.ToListAsync();
        }
        // End of GetAllCategories method

        // Beginning of GetCategoryById method
        public async Task<Category?> GetCategoryById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
        // End of GetCategoryById method

        // Beginning of CreateCategory method
        public async Task<(bool Success, string Message, Category? Category)> CreateCategory(
            Category category,
            int organiserId)
        {
            // Find the event that the category belongs to
            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == category.EventID);

            if (eventItem == null)
            {
                return (
                    false,
                    "The selected event does not exist.",
                    null);
            }

            // Check that the organiser owns the event
            if (eventItem.OrganiserID != organiserId)
            {
                return (
                    false,
                    "You can only create categories for your own events.",
                    null);
            }

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return (
                true,
                "Category created successfully.",
                category);
        }
        // End of CreateCategory method

        // Beginning of UpdateCategory method
        public async Task<(bool Success, string Message)> UpdateCategory(
            Category category,
            int organiserId)
        {
            var existingCategory =
                await _context.Categories.FindAsync(category.CategoryID);

            if (existingCategory == null)
            {
                return (
                    false,
                    "The category does not exist.");
            }

            // Find the event currently belonging to the category
            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e =>
                    e.EventID == existingCategory.EventID);

            if (eventItem == null)
            {
                return (
                    false,
                    "The event associated with this category does not exist.");
            }

            // Check that the organiser owns the event
            if (eventItem.OrganiserID != organiserId)
            {
                return (
                    false,
                    "You can only update categories for your own events.");
            }

            // Check that the new event belongs to the organiser
            var newEvent = await _context.Events
                .FirstOrDefaultAsync(e =>
                    e.EventID == category.EventID);

            if (newEvent == null)
            {
                return (
                    false,
                    "The selected event does not exist.");
            }

            if (newEvent.OrganiserID != organiserId)
            {
                return (
                    false,
                    "You can only move categories to your own events.");
            }

            existingCategory.EventID = category.EventID;
            existingCategory.CategoryName = category.CategoryName;
            existingCategory.Description = category.Description;

            await _context.SaveChangesAsync();

            return (
                true,
                "Category updated successfully.");
        }
        // End of UpdateCategory method

        // Beginning of DeleteCategory method
        public async Task<(bool Success, string Message)> DeleteCategory(
            int id,
            int organiserId)
        {
            var category =
                await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return (
                    false,
                    "The category does not exist.");
            }

            // Find the event that owns the category
            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e =>
                    e.EventID == category.EventID);

            if (eventItem == null)
            {
                return (
                    false,
                    "The event associated with this category does not exist.");
            }

            // Check that the organiser owns the event
            if (eventItem.OrganiserID != organiserId)
            {
                return (
                    false,
                    "You can only delete categories for your own events.");
            }

            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();

            return (
                true,
                "Category deleted successfully.");
        }
        // End of DeleteCategory method
    }
    // End of CategoryService class
}
// End of Category service namespace
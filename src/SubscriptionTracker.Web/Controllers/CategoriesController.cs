using Microsoft.AspNetCore.Mvc;
using SubscriptionTracker.Web.Models.Domain;
using SubscriptionTracker.Web.Models.ViewModels;
using SubscriptionTracker.Web.Services.Interfaces;

namespace SubscriptionTracker.Web.Controllers
{
    /// <summary>
    /// Controller for managing categories
    /// </summary>
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        /// <summary>
        /// Initializes a new instance of the CategoriesController class
        /// </summary>
        /// <param name="categoryService">The category service</param>
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Displays the list of categories
        /// </summary>
        /// <returns>View with categories</returns>
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var viewModels = categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Color = c.Color,
                SubscriptionCount = c.Subscriptions.Count,
                TotalCost = c.Subscriptions.Sum(s => s.Cost)
            });

            return View(viewModels);
        }

        /// <summary>
        /// Displays the create category form
        /// </summary>
        /// <returns>View with form</returns>
        public IActionResult Create()
        {
            return View(new CategoryFormViewModel());
        }

        /// <summary>
        /// Handles the category creation
        /// </summary>
        /// <param name="model">The category form data</param>
        /// <returns>Redirect to index on success, view with errors otherwise</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var category = new Category
                {
                    Name = model.Name,
                    Color = model.Color
                };

                await _categoryService.CreateCategoryAsync(category);
                TempData["SuccessMessage"] = "Category created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        /// <summary>
        /// Displays the edit category form
        /// </summary>
        /// <param name="id">The category ID</param>
        /// <returns>View with form</returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var viewModel = new CategoryFormViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Color = category.Color
            };

            return View(viewModel);
        }

        /// <summary>
        /// Handles the category update
        /// </summary>
        /// <param name="id">The category ID</param>
        /// <param name="model">The category form data</param>
        /// <returns>Redirect to index on success, view with errors otherwise</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CategoryFormViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var category = new Category
                {
                    Id = id,
                    Name = model.Name,
                    Color = model.Color
                };

                await _categoryService.UpdateCategoryAsync(category);
                TempData["SuccessMessage"] = "Category updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        /// <summary>
        /// Handles the category deletion
        /// </summary>
        /// <param name="id">The category ID</param>
        /// <returns>Redirect to index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                TempData["SuccessMessage"] = "Category deleted successfully.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
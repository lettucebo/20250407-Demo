using Microsoft.AspNetCore.Mvc;
using SubscriptionTracker.Web.Models.Domain;
using SubscriptionTracker.Web.Models.ViewModels;
using SubscriptionTracker.Web.Services.Interfaces;

namespace SubscriptionTracker.Web.Controllers
{
    /// <summary>
    /// Controller for managing subscriptions
    /// </summary>
    public class SubscriptionsController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly ICategoryService _categoryService;

        /// <summary>
        /// Initializes a new instance of the SubscriptionsController class
        /// </summary>
        /// <param name="subscriptionService">The subscription service</param>
        /// <param name="categoryService">The category service</param>
        public SubscriptionsController(
            ISubscriptionService subscriptionService,
            ICategoryService categoryService)
        {
            _subscriptionService = subscriptionService;
            _categoryService = categoryService;
        }

        /// <summary>
        /// Displays the subscription dashboard
        /// </summary>
        /// <returns>View with dashboard data</returns>
        public async Task<IActionResult> Index()
        {
            var allSubscriptions = await _subscriptionService.GetAllSubscriptionsAsync();
            var upcomingSubscriptions = await _subscriptionService.GetUpcomingSubscriptionsAsync();
            var categories = await _categoryService.GetAllCategoriesAsync();

            var viewModel = new SubscriptionDashboardViewModel
            {
                TotalMonthlyCost = await _subscriptionService.GetTotalSubscriptionCostAsync(),
                UpcomingPayments = upcomingSubscriptions.Select(MapToViewModel),
                CategoryBreakdown = categories.Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Color = c.Color,
                    TotalCost = c.Subscriptions.Sum(s => s.Cost),
                    SubscriptionCount = c.Subscriptions.Count
                }),
                ActiveSubscriptions = allSubscriptions.Select(MapToViewModel)
            };

            return View(viewModel);
        }

        /// <summary>
        /// Displays the create subscription form
        /// </summary>
        /// <returns>View with form</returns>
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var viewModel = new SubscriptionFormViewModel
            {
                PaymentTime = DateTimeOffset.UtcNow,
                AvailableCategories = categories.Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Color = c.Color
                })
            };

            return View(viewModel);
        }

        /// <summary>
        /// Handles the subscription creation
        /// </summary>
        /// <param name="model">The subscription form data</param>
        /// <returns>Redirect to index on success, view with errors otherwise</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubscriptionFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableCategories = await GetAvailableCategories();
                return View(model);
            }

            try
            {
                var subscription = new Subscription
                {
                    Name = model.Name,
                    Cost = model.Cost,
                    PaymentTime = model.PaymentTime,
                    CategoryId = model.CategoryId
                };

                await _subscriptionService.CreateSubscriptionAsync(subscription);
                TempData["SuccessMessage"] = "Subscription created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                model.AvailableCategories = await GetAvailableCategories();
                return View(model);
            }
        }

        /// <summary>
        /// Displays the edit subscription form
        /// </summary>
        /// <param name="id">The subscription ID</param>
        /// <returns>View with form</returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var subscription = await _subscriptionService.GetSubscriptionByIdAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }

            var viewModel = new SubscriptionFormViewModel
            {
                Id = subscription.Id,
                Name = subscription.Name,
                Cost = subscription.Cost,
                PaymentTime = subscription.PaymentTime,
                CategoryId = subscription.CategoryId,
                AvailableCategories = await GetAvailableCategories()
            };

            return View(viewModel);
        }

        /// <summary>
        /// Handles the subscription update
        /// </summary>
        /// <param name="id">The subscription ID</param>
        /// <param name="model">The subscription form data</param>
        /// <returns>Redirect to index on success, view with errors otherwise</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, SubscriptionFormViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                model.AvailableCategories = await GetAvailableCategories();
                return View(model);
            }

            try
            {
                var subscription = new Subscription
                {
                    Id = id,
                    Name = model.Name,
                    Cost = model.Cost,
                    PaymentTime = model.PaymentTime,
                    CategoryId = model.CategoryId
                };

                await _subscriptionService.UpdateSubscriptionAsync(subscription);
                TempData["SuccessMessage"] = "Subscription updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentException)
            {
                ModelState.AddModelError("", ex.Message);
                model.AvailableCategories = await GetAvailableCategories();
                return View(model);
            }
        }

        /// <summary>
        /// Handles the subscription deletion
        /// </summary>
        /// <param name="id">The subscription ID</param>
        /// <returns>Redirect to index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _subscriptionService.DeleteSubscriptionAsync(id);
                TempData["SuccessMessage"] = "Subscription deleted successfully.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<CategoryViewModel>> GetAvailableCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Color = c.Color
            });
        }

        private static SubscriptionViewModel MapToViewModel(Subscription subscription)
        {
            return new SubscriptionViewModel
            {
                Id = subscription.Id,
                Name = subscription.Name,
                Cost = subscription.Cost,
                PaymentTime = subscription.PaymentTime,
                DaysUntilPayment = (subscription.PaymentTime - DateTimeOffset.UtcNow).Days,
                CategoryId = subscription.CategoryId,
                CategoryName = subscription.Category?.Name ?? string.Empty,
                CategoryColor = subscription.Category?.Color ?? "#000000"
            };
        }
    }
}
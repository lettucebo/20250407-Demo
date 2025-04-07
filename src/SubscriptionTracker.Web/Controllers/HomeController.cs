using Microsoft.AspNetCore.Mvc;
using SubscriptionTracker.Web.Models.ViewModels;
using SubscriptionTracker.Web.Services.Interfaces;
using System.Diagnostics;

namespace SubscriptionTracker.Web.Controllers
{
    /// <summary>
    /// Main controller for the application
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly ICategoryService _categoryService;

        /// <summary>
        /// Initializes a new instance of the HomeController class
        /// </summary>
        /// <param name="subscriptionService">The subscription service</param>
        /// <param name="categoryService">The category service</param>
        public HomeController(
            ISubscriptionService subscriptionService,
            ICategoryService categoryService)
        {
            _subscriptionService = subscriptionService;
            _categoryService = categoryService;
        }

        /// <summary>
        /// Displays the main dashboard
        /// </summary>
        /// <returns>View with dashboard data</returns>
        public async Task<IActionResult> Index()
        {
            var subscriptions = await _subscriptionService.GetAllSubscriptionsAsync();
            var upcomingSubscriptions = await _subscriptionService.GetUpcomingSubscriptionsAsync(7); // Next 7 days
            var categories = await _categoryService.GetAllCategoriesAsync();
            var totalCost = await _subscriptionService.GetTotalSubscriptionCostAsync();

            var viewModel = new SubscriptionDashboardViewModel
            {
                TotalMonthlyCost = totalCost,
                UpcomingPayments = upcomingSubscriptions.Select(s => new SubscriptionViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Cost = s.Cost,
                    PaymentTime = s.PaymentTime,
                    DaysUntilPayment = (s.PaymentTime - DateTimeOffset.UtcNow).Days,
                    CategoryId = s.CategoryId,
                    CategoryName = s.Category?.Name ?? string.Empty,
                    CategoryColor = s.Category?.Color ?? "#000000"
                }),
                CategoryBreakdown = categories.Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Color = c.Color,
                    TotalCost = c.Subscriptions.Sum(s => s.Cost),
                    SubscriptionCount = c.Subscriptions.Count
                })
            };

            return View(viewModel);
        }

        /// <summary>
        /// Displays the privacy policy
        /// </summary>
        /// <returns>Privacy policy view</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Handles errors
        /// </summary>
        /// <returns>Error view</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}

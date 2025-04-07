using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SubscriptionTracker.Web.Models.Domain;
using SubscriptionTracker.Web.Repositories.Interfaces;
using SubscriptionTracker.Web.Services.Implementations;

namespace SubscriptionTracker.Tests.Services
{
    /// <summary>
    /// Tests for SubscriptionService
    /// </summary>
    [TestClass]
    public class SubscriptionServiceTests
    {
        private readonly Mock<ISubscriptionRepository> _subscriptionRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly SubscriptionService _subscriptionService;

        /// <summary>
        /// Initializes test class
        /// </summary>
        public SubscriptionServiceTests()
        {
            _subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _subscriptionService = new SubscriptionService(
                _subscriptionRepositoryMock.Object,
                _categoryRepositoryMock.Object);
        }

        [TestMethod]
        public async Task CreateSubscriptionAsync_WithValidSubscription_ShouldSucceed()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category
            {
                Id = categoryId,
                Name = "Test Category",
                Color = "#FF0000"
            };

            var subscription = new Subscription
            {
                Name = "Test Subscription",
                Cost = 9.99m,
                PaymentTime = DateTimeOffset.UtcNow,
                CategoryId = categoryId
            };

            _categoryRepositoryMock.Setup(x => x.GetByIdWithSubscriptionsAsync(categoryId))
                .ReturnsAsync(category);

            _subscriptionRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Subscription>()))
                .ReturnsAsync(subscription);

            // Act
            var result = await _subscriptionService.CreateSubscriptionAsync(subscription);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(subscription.Name);
            result.Cost.Should().Be(subscription.Cost);
            result.CategoryId.Should().Be(categoryId);
            _subscriptionRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Subscription>()), Times.Once);
            _subscriptionRepositoryMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [TestMethod]
        public async Task CreateSubscriptionAsync_WithInvalidCategory_ShouldThrowArgumentException()
        {
            // Arrange
            var subscription = new Subscription
            {
                Name = "Test Subscription",
                Cost = 9.99m,
                PaymentTime = DateTimeOffset.UtcNow,
                CategoryId = Guid.NewGuid()
            };

            _categoryRepositoryMock.Setup(x => x.GetByIdWithSubscriptionsAsync(subscription.CategoryId))
                .ReturnsAsync((Category?)null);

            // Act
            var action = () => _subscriptionService.CreateSubscriptionAsync(subscription);

            // Assert
            await action.Should().ThrowAsync<ArgumentException>()
                .WithMessage($"Category with ID {subscription.CategoryId} not found.");
        }

        [TestMethod]
        public async Task GetUpcomingSubscriptionsAsync_ShouldReturnOrderedSubscriptions()
        {
            // Arrange
            var now = DateTimeOffset.UtcNow;
            var subscriptions = new List<Subscription>
            {
                new() { Id = Guid.NewGuid(), Name = "Sub 1", PaymentTime = now.AddDays(5) },
                new() { Id = Guid.NewGuid(), Name = "Sub 2", PaymentTime = now.AddDays(2) },
                new() { Id = Guid.NewGuid(), Name = "Sub 3", PaymentTime = now.AddDays(10) }
            };

            _subscriptionRepositoryMock.Setup(x => x.GetByDateRange(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(subscriptions.AsQueryable());

            // Act
            var result = await _subscriptionService.GetUpcomingSubscriptionsAsync(30);

            // Assert
            result.Should().HaveCount(3);
            result.Should().BeInAscendingOrder(s => s.PaymentTime);
        }

        [TestMethod]
        public async Task GetTotalSubscriptionCostAsync_ShouldReturnCorrectSum()
        {
            // Arrange
            var subscriptions = new List<Subscription>
            {
                new() { Id = Guid.NewGuid(), Name = "Sub 1", Cost = 10.99m },
                new() { Id = Guid.NewGuid(), Name = "Sub 2", Cost = 5.99m },
                new() { Id = Guid.NewGuid(), Name = "Sub 3", Cost = 15.99m }
            };

            _subscriptionRepositoryMock.Setup(x => x.GetActiveWithCategories())
                .Returns(subscriptions.AsQueryable());

            // Act
            var result = await _subscriptionService.GetTotalSubscriptionCostAsync();

            // Assert
            result.Should().Be(32.97m);
        }
    }
}
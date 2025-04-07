using FluentAssertions;
using Moq;
using SubscriptionTracker.Web.Models.Domain;
using SubscriptionTracker.Web.Repositories.Interfaces;
using SubscriptionTracker.Web.Services.Implementations;

namespace SubscriptionTracker.Tests.Services
{
    /// <summary>
    /// Tests for CategoryService
    /// </summary>
    [TestClass]
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly CategoryService _categoryService;

        /// <summary>
        /// Initializes test class
        /// </summary>
        public CategoryServiceTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _categoryService = new CategoryService(_categoryRepositoryMock.Object);
        }

        [TestMethod]
        public async Task CreateCategoryAsync_WithValidCategory_ShouldSucceed()
        {
            // Arrange
            var category = new Category
            {
                Name = "Test Category",
                Color = "#FF0000"
            };

            _categoryRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Category>()))
                .ReturnsAsync(category);

            // Act
            var result = await _categoryService.CreateCategoryAsync(category);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(category.Name);
            result.Color.Should().Be(category.Color);
            _categoryRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Category>()), Times.Once);
            _categoryRepositoryMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [TestMethod]
        public async Task CreateCategoryAsync_WithInvalidColor_ShouldThrowArgumentException()
        {
            // Arrange
            var category = new Category
            {
                Name = "Test Category",
                Color = "InvalidColor"
            };

            // Act
            var action = () => _categoryService.CreateCategoryAsync(category);

            // Assert
            await action.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Category color must be a valid hex color code (e.g., #FF0000).");
        }

        [TestMethod]
        public async Task GetAllCategoriesAsync_ShouldReturnAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new() { Id = Guid.NewGuid(), Name = "Category 1", Color = "#FF0000" },
                new() { Id = Guid.NewGuid(), Name = "Category 2", Color = "#00FF00" }
            };

            _categoryRepositoryMock.Setup(x => x.GetActiveWithSubscriptions())
                .Returns(categories.AsQueryable());

            // Act
            var result = await _categoryService.GetAllCategoriesAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(categories);
        }

        [TestMethod]
        public async Task DeleteCategoryAsync_WithActiveSubscriptions_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category
            {
                Id = categoryId,
                Name = "Test Category",
                Color = "#FF0000",
                Subscriptions = new List<Subscription>
                {
                    new() { Id = Guid.NewGuid(), Name = "Test Subscription" }
                }
            };

            _categoryRepositoryMock.Setup(x => x.GetByIdWithSubscriptionsAsync(categoryId))
                .ReturnsAsync(category);

            // Act
            var action = () => _categoryService.DeleteCategoryAsync(categoryId);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Cannot delete category with active subscriptions.");
        }
    }
}
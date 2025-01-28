using KKBundle.API.Controllers;
using KKBundle.API.Controllers.PricingResources;
using KKBundle.API.Domains.Pricing;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace KKBundle.Api.Tests
{
    public class PricingControllerTests
    {
        private Mock<IPricingService> mockPricingService = new Mock<IPricingService>();

        [Fact]
        public void CreateOffer_When_requestIsValid_Returns_validPricingData()
        {
            // Arrange
            var topics = new Dictionary<string, int>
            {
                { "reading", 20 },
                { "math", 50 },
                { "science", 30 }
            };

            var request = new TeacherRequest(topics);

            var expectedPricing = new List<Pricing>
            {
                new ("provider_a", 8.0m),
                new ("provider_b", 5.0m)
            };

            mockPricingService
                .Setup(service => service.CalculateOffer(It.IsAny<PricingQuery>()))
                .Returns(expectedPricing);

            var controller = new PricingController(mockPricingService.Object);

            // Act
            var result = controller.CreateOffer(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(controller.CreateOffer), createdResult.ActionName);
            Assert.Equal(expectedPricing, createdResult.Value);
        }

        [Fact]
        public void CreateOffer_ReturnsBadRequest_WhenRequestIsInvalid()
        {
            // Arrange
            var controller = new PricingController(mockPricingService.Object);
            controller.ModelState.AddModelError("Topics", "Topics are required");

            // Act
            var result = controller.CreateOffer(new TeacherRequest(null));

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
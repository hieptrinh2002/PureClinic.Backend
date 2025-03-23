using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using PureLifeClinic.API.ActionFilters;
using PureLifeClinic.Core.Interfaces.IServices;
using Xunit;

namespace PeruLifeClinic.Api.Tests.ActionFilters
{
    public class ValidateInputViewModelFilterTests
    {
        private readonly Mock<IValidationService> _mockValidationService;
        private readonly ValidateInputViewModelFilter _filter;

        public ValidateInputViewModelFilterTests()
        {
            _mockValidationService = new Mock<IValidationService>();
            _filter = new ValidateInputViewModelFilter(_mockValidationService.Object);
        }

        [Fact]
        public async Task OnActionExecutionAsync_ShouldContinue_WhenValidationIsSuccessful()
        {
            // Arrange
            var actionArguments = new Dictionary<string, object?>
            {
                { "model", new TestViewModel() }
            };
            var context = new ActionExecutingContext(
                new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
                new List<IFilterMetadata>(),
                actionArguments,
                new object());

            var next = new Mock<ActionExecutionDelegate>();
            next.Setup(n => n())
                .Returns(Task.FromResult(new ActionExecutedContext(
                    new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
                    new List<IFilterMetadata>(),
                    new object())
                ));

            var validationResult = new ValidationResult();
            _mockValidationService.Setup(v => v.ValidateAsync(It.IsAny<object>()))
                .ReturnsAsync(validationResult);

            // Act
            await _filter.OnActionExecutionAsync(context, next.Object);

            // Assert
            next.Verify(n => n(), Times.Once);
        }

        [Fact]
        public async Task OnActionExecutionAsync_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var actionArguments = new Dictionary<string, object?>
            {
                { "model", new TestViewModel() }
            };
            var context = new ActionExecutingContext(
                new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
                new List<IFilterMetadata>(),
                actionArguments,
                new object());

            var next = new Mock<ActionExecutionDelegate>();

            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("PropertyName", "ErrorMessage")
            });
            _mockValidationService.Setup(v => v.ValidateAsync(It.IsAny<object>()))
                .ReturnsAsync(validationResult);

            // Act
            await _filter.OnActionExecutionAsync(context, next.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(context.Result);
        }

        [Fact]
        public async Task OnActionExecutionAsync_ShouldSkipNullArguments()
        {
            // Arrange
            var actionArguments = new Dictionary<string, object?>
            {
                { "model", null }
            };
            var context = new ActionExecutingContext(
                new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
                new List<IFilterMetadata>(),
                actionArguments,
                new object());

            var next = new Mock<ActionExecutionDelegate>();
            next.Setup(n => n())
                .Returns(Task.FromResult(
                    new ActionExecutedContext(
                        new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
                        new List<IFilterMetadata>(),
                        new object()))
                );

            // Act
            await _filter.OnActionExecutionAsync(context, next.Object);

            // Assert
            next.Verify(n => n(), Times.Once);
        }

        //[Fact]
        //public async Task OnActionExecutionAsync_ShouldSkipNonViewModelArguments()
        //{
        //    // Arrange
        //    var actionArguments = new Dictionary<string, object?>
        //    {
        //        { "model", new NonViewModel() }
        //    };
        //    var context = new ActionExecutingContext(
        //        new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
        //        new List<IFilterMetadata>(),
        //        actionArguments,
        //        new object());

        //    var next = new Mock<ActionExecutionDelegate>();
        //    next.Setup(n => n())
        //        .Returns(Task.FromResult(
        //            new ActionExecutedContext(
        //                new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
        //                new List<IFilterMetadata>(),
        //                new object()))
        //        );

        //    // Act
        //    await _filter.OnActionExecutionAsync(context, next.Object);

        //    // Assert
        //    next.Verify(n => n(), Times.Once);
        //}

        private class TestViewModel { }

        private class NonViewModel { }
    }
}
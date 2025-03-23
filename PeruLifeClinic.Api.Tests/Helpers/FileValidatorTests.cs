using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using PureLifeClinic.API.Helpers;
using Xunit;

namespace PeruLifeClinic.Api.Tests.Helpers
{
    public class FileValidatorTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly FileValidator _fileValidator;

        public FileValidatorTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _fileValidator = new FileValidator(_mockConfiguration.Object);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenFileIsEmpty()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(0);

            // Act
            var result = _fileValidator.IsValid(fileMock.Object);

            // Assert
            Assert.False(result.isValid);
            Assert.Equal("File is empty.", result.errorMessage);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenFileExtensionIsMissing()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1024);
            fileMock.Setup(f => f.FileName).Returns("file");

            // Act
            var result = _fileValidator.IsValid(fileMock.Object);

            // Assert
            Assert.False(result.isValid);
            Assert.Equal("File extension is missing.", result.errorMessage);
        }

        [Theory]
        [InlineData(".jpg", 5 * 1024 * 1024, false, "File size exceeds the 4MB limit for images and documents.")]
        [InlineData(".jpg", 3 * 1024 * 1024, true, "")]
        [InlineData(".mp4", 101 * 1024 * 1024, false, "File size exceeds the 100MB limit for videos.")]
        [InlineData(".mp4", 99 * 1024 * 1024, true, "")]
        public void IsValid_ShouldValidateFileBasedOnExtensionAndSize(string extension, long size, bool expectedIsValid, string expectedErrorMessage)
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(size);
            fileMock.Setup(f => f.FileName).Returns($"file{extension}");

            // Act
            var result = _fileValidator.IsValid(fileMock.Object);

            // Assert
            Assert.Equal(expectedIsValid, result.isValid);
            Assert.Equal(expectedErrorMessage, result.errorMessage);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenFileExtensionIsInvalid()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1024);
            fileMock.Setup(f => f.FileName).Returns("file.invalid");

            // Act
            var result = _fileValidator.IsValid(fileMock.Object);

            // Assert
            Assert.False(result.isValid);
            Assert.Equal("Invalid file extension.", result.errorMessage);
        }
    }
}
using InterviewApp.Models;
using InterviewApp.Queries;
using InterviewApp.Services.Greetings;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace InterviewApp.Tests
{
    public class GreetingServiceTests
    {
        private readonly Mock<ILogger<GreetingService>> _loggerMock;
        private readonly Mock<ISender> _senderMock;

        public GreetingServiceTests()
        {
            _loggerMock = new Mock<ILogger<GreetingService>>();
            _senderMock = new Mock<ISender>();

            _senderMock
                .Setup(x => x.Send(
                    It.IsAny<GetTimeGreetingQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync("Good morning");
        }

        private GreetingService CreateService(
            string language,
            string message = "Welcome to the chat application!")
        {
            var options = Options.Create(new GreetingOptions
            {
                Language = language,
                Message = message
            });

            return new GreetingService(
                options,
                _loggerMock.Object,
                _senderMock.Object);
        }

        [Theory]
        [InlineData("English", "Hello")]
        [InlineData("Sepedi", "Dumela")]
        [InlineData("Afrikaans", "Hallo")]
        [InlineData("Zulu", "Sawubona")]
        public async Task GetGreeting_ValidLanguage_ReturnsCorrectGreeting(
            string language,
            string expectedGreeting)
        {
            // Arrange
            var service = CreateService(language);

            // Act
            var result = await service.GetGreeting();

            // Assert
            Assert.Contains(expectedGreeting, result);
            Assert.Contains("Good morning", result);
        }

        [Theory]
        [InlineData("english")]
        [InlineData("ENGLISH")]
        [InlineData("eNgLiSh")]
        [InlineData(" English ")]
        public async Task GetGreeting_LanguageIsCaseInsensitive_ReturnsEnglish(
            string language)
        {
            // Arrange
            var service = CreateService(language);

            // Act
            var result = await service.GetGreeting();

            // Assert
            Assert.Contains("Hello!", result);
        }

        [Fact]
        public async Task GetGreeting_UnsupportedLanguage_FallsBackToEnglish()
        {
            // Arrange
            var service = CreateService("French");

            // Act
            var result = await service.GetGreeting();

            // Assert
            Assert.Contains("Hello!", result);
            Assert.Contains("Welcome to the chat application!", result);
        }

        [Theory]
        [InlineData("", "Welcome")]
        [InlineData(" ", "Welcome")]
        [InlineData("English", "")]
        [InlineData("English", " ")]
        public async Task GetGreeting_InvalidConfiguration_ReturnsEmptyString(
            string language,
            string message)
        {
            // Arrange
            var service = CreateService(language, message);

            // Act
            var result = await service.GetGreeting();

            // Assert
            Assert.Equal(string.Empty, result);

            _senderMock.Verify(
                x => x.Send(
                    It.IsAny<GetTimeGreetingQuery>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task GetGreeting_ValidConfiguration_CallsMediatorOnce()
        {
            // Arrange
            var service = CreateService("English");

            // Act
            await service.GetGreeting();

            // Assert
            _senderMock.Verify(
                x => x.Send(
                    It.IsAny<GetTimeGreetingQuery>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetGreeting_CancellationToken_PassedToMediator()
        {
            // Arrange
            var service = CreateService("English");
            using var cts = new CancellationTokenSource();

            // Act
            await service.GetGreeting(cts.Token);

            // Assert
            _senderMock.Verify(
                x => x.Send(
                    It.IsAny<GetTimeGreetingQuery>(),
                    cts.Token),
                Times.Once);
        }

        [Fact]
        public async Task GetGreeting_MediatorThrowsException_ReturnsEmptyString()
        {
            // Arrange
            _senderMock
                .Setup(x => x.Send(
                    It.IsAny<GetTimeGreetingQuery>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException(
                    "Mediator failure"));

            var service = CreateService("English");

            // Act
            var result = await service.GetGreeting();

            // Assert
            Assert.Equal(string.Empty, result);
        }
    }
}
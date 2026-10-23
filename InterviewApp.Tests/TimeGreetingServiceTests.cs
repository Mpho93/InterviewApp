
using System;
using InterviewApp.Services.TimeGreetings;
using Moq;
using Xunit;

namespace InterviewApp.Tests
{
    public class TimeGreetingServiceTests
    {
        [Theory]
        [InlineData(11, 59, "Good morning")]
        [InlineData(12, 00, "Good afternoon")]
        [InlineData(17, 59, "Good afternoon")]
        [InlineData(18, 00, "Good evening")]
        public void GetTimeGreeting_AtSpecificTime_ReturnsCorrectGreeting(
            int hour,
            int minute,
            string expectedGreeting)
        {
            // Arrange
            var fixedTime = new DateTimeOffset(
                2026, 10, 23,
                hour, minute, 0,
                TimeSpan.Zero);

            var timeProviderMock = new Mock<TimeProvider>();

            timeProviderMock
                .Setup(x => x.GetUtcNow())
                .Returns(fixedTime);

            timeProviderMock
                .Setup(x => x.LocalTimeZone)
                .Returns(TimeZoneInfo.Utc);

            var service = new TimeGreetingService(timeProviderMock.Object);

            // Act
            string actualGreeting = service.GetTimeGreeting();

            // Assert
            Assert.Equal(expectedGreeting, actualGreeting);
        }
    }
}

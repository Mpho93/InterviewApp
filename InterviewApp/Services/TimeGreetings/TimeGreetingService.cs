using System;

namespace InterviewApp.Services.TimeGreetings
{
    public class TimeGreetingService : ITimeGreetingService
    {
        private readonly TimeProvider _timeProvider;

        public TimeGreetingService(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public string GetTimeGreeting()
        {
            int hour = _timeProvider.GetLocalNow().Hour;

            return hour switch
            {
                < 12 => "Good morning",
                < 18 => "Good afternoon",
                _ => "Good evening"
            };
        }
    }
}

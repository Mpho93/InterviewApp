using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewApp.Services.TimeGreetings
{
    public class TimeGreetingService : ITimeGreetingService
    {
        public string GetTimeGreeting()
        {
            var hour = DateTime.Now.Hour;
            if (hour < 12)
            {
                return "Good morning!";
            }
            else if (hour < 18)
            {
                return "Good afternoon!";
            }
            else
            {
                return "Good evening!";
            }
        }
    }
}

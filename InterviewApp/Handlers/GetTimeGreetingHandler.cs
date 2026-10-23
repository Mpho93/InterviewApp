using InterviewApp.Queries;
using InterviewApp.Services.TimeGreetings;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InterviewApp.Handlers
{
    public class GetTimeGreetingHandler(ITimeGreetingService timeGreetingService) : IRequestHandler<GetTimeGreetingQuery, string>
    {
        private readonly ITimeGreetingService _timeGreetingService = timeGreetingService;

        public Task<string> Handle(GetTimeGreetingQuery request, CancellationToken cancellationToken)
        {
            string timeGreeting = _timeGreetingService.GetTimeGreeting();
            return Task.FromResult(timeGreeting);
        }
    }
}

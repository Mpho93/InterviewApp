using InterviewApp.Commands;
using InterviewApp.Services.Greetings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace InterviewApp.Handlers
{
    public class GreetUserHandler(IGreetingService greetingService) : IRequestHandler<GreetUserCommand, string>
    {
        private readonly IGreetingService _greetingService = greetingService;

        public async Task<string> Handle(GreetUserCommand request, CancellationToken cancellationToken)
        {
            string message = await _greetingService.GetGreeting(cancellationToken);
            return message;
        }
    }
}

using MediatR;

namespace InterviewApp.Commands
{
    public record GreetUserCommand() : IRequest<string>;
    
}

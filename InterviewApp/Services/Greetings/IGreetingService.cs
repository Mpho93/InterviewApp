using System.Threading;
using System.Threading.Tasks;

namespace InterviewApp.Services.Greetings;

public interface IGreetingService
{
    //void Run();
    Task<string> GetGreeting(CancellationToken cancellationToken = default);
}
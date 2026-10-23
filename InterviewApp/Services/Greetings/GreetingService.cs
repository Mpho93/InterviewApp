using InterviewApp.Models;
using InterviewApp.Queries;
using InterviewApp.Services.TimeGreetings;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InterviewApp.Services.Greetings
{
    public class GreetingService(IOptions<GreetingOptions> options, ILogger<GreetingService> logger, /*ITimeGreetingService timeGreetingService,*/ ISender sender) : IGreetingService
    {
        private readonly GreetingOptions _options = options.Value;
        private readonly ILogger<GreetingService> _logger = logger;
        //private readonly ITimeGreetingService _timeGreetingService = timeGreetingService;
        private readonly ISender _sender = sender;

        public async Task<string> GetGreeting(CancellationToken cancellationToken = default) 
        {
            _logger.LogInformation("Greeting service started.");
            try {

                if (!string.IsNullOrWhiteSpace(_options.Language) && !string.IsNullOrWhiteSpace(_options.Message))
                { 
                    string timeGreeting = await _sender.Send(new GetTimeGreetingQuery(), cancellationToken);
                    string greeting = _options.Language switch
                    {
                        "English" => _options.Language,
                        "Sepedi" => "Dumela",
                        _ => "Hello"
                    };
                    string message = _options.Language == "English" ? timeGreeting+""+_options.Message : timeGreeting + "" + "Re a go amogela mo tirišong ya poledišano!";
                    Console.WriteLine(message);
                    _logger.LogInformation("Greeting service ran successfully with message: {Message} and language: {Language}", message, _options.Language);

                    return message;
                }
                else
                {
                    _logger.LogWarning("Greeting service did not run because the message is empty or whitespace.");

                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while running the greeting service.");
                return string.Empty;
            }
        }
    }
}
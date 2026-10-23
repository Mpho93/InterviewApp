using InterviewApp.Models;
using InterviewApp.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InterviewApp.Services.Greetings
{
    public class GreetingService(IOptions<GreetingOptions> options, ILogger<GreetingService> logger, ISender sender) : IGreetingService
    {
        private readonly GreetingOptions _options = options.Value;
        private readonly ILogger<GreetingService> _logger = logger;
        private readonly ISender _sender = sender;

        public async Task<string> GetGreeting(CancellationToken cancellationToken = default) 
        {
            _logger.LogInformation("Greeting service started.");
            try {

                    if (string.IsNullOrWhiteSpace(_options.Language) || string.IsNullOrWhiteSpace(_options.Message))
                    {
                        _logger.LogWarning("Greeting service could not run because language or message is empty.");

                        return string.Empty;    
                    }

                        string language = _options.Language.Trim().ToLowerInvariant();

                        string timeGreeting = await _sender.Send(new GetTimeGreetingQuery(), cancellationToken);

                        string greeting = language switch
                        {
                            "english" => "Hello",
                            "sepedi" => "Dumela",
                            "afrikaans" => "Hallo",
                            "zulu" => "Sawubona",
                            _ => "Hello"
                        };

                        string welcomeMessage = language switch
                        {
                            "english" => _options.Message,
                            "sepedi" => "Re a go amogela mo tirišong ya poledišano!",
                            "afrikaans" => "Welkom by die geselstoepassing!",
                            "zulu" => "Siyakwamukela ohlelweni lokuxoxa!",
                            _ => _options.Message
                        };

                        string message = $"{timeGreeting} {greeting}! {welcomeMessage}";

                         _logger.LogInformation("Greeting service ran successfully with message: {Message} and language: {Language}", message, _options.Language);

                    return message;
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while running the greeting service.");
                return string.Empty;
            }
        }
    }
}
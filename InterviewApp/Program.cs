using InterviewApp.Commands;
using InterviewApp.Models;
using InterviewApp.Services.Greetings;
using InterviewApp.Services.TimeGreetings;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((_, services) => {
                // Register options
                services.Configure<GreetingOptions>(
                    _.Configuration.GetSection("Greeting")
                );

                // Register existing services
                services.AddTransient<IGreetingService, GreetingService>();
                services.AddTransient<ITimeGreetingService, TimeGreetingService>();

                services.AddSingleton(TimeProvider.System);
                services.AddScoped<ITimeGreetingService, TimeGreetingService>();

                // Register MediatR
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            })
            .Build();

        var mediator = host.Services.GetRequiredService<IMediator>();
        string message = await mediator.Send(new GreetUserCommand());

        Console.WriteLine(message);

        await host.RunAsync();
    }
}
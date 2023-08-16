using Complexidade.Algoritmo.Infrastructure.Services;
using Complexidade.Algoritmos.Domain.Interfaces;
using Complexidade.Algoritmos.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
ConfigureDependencyInjectionContainer(builder.Services);
var host = builder.Build();

var word = builder
    .Configuration
    .GetSection("AutomatonInput")
    .Get<IEnumerable<string>>() ?? Enumerable.Empty<string>();

using var scope = host.Services.CreateScope();
var service = scope.ServiceProvider.GetRequiredService<IDeterministicFiniteAutomatonOrchestrator>();

var currentDirectory = Directory.GetCurrentDirectory();
var path = Path.Combine(currentDirectory, "AutomatonDescriptor.txt");
service.Execute(path, word);

static void ConfigureDependencyInjectionContainer(IServiceCollection services)
{
    services.AddScoped<IDeterministicFiniteAutomatonOrchestrator, DeterministicFiniteAutomatonOrchestrator>();
    services.AddScoped<IFileManagementService, FileManagementService>();
    services.AddScoped<IParser, Parser>();
    services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddConsole();
    });
}
using Complexidade.Algoritmos.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Complexidade.Algoritmos.Domain.Services;

public class NonDeterministicFiniteAutomatonOrchestrator : INonDeterministicFiniteAutomatonOrchestrator
{
    private readonly ILogger<NonDeterministicFiniteAutomatonOrchestrator> _logger;
    private readonly ILexer _lexer;
    private readonly IParser _parser;

    public NonDeterministicFiniteAutomatonOrchestrator(
        ILogger<NonDeterministicFiniteAutomatonOrchestrator> logger,
        ILexer lexer,
        IParser parser)
    {
        _logger = logger;
        _lexer = lexer;
        _parser = parser;
    }

    public void Execute(string path, IEnumerable<string> word)
    {
        var tokens = _lexer.Execute(path);
        var nonDeterministicAutomaton = _parser.Execute(tokens);
        var deterministicAutomaton = nonDeterministicAutomaton.ConvertToDeterministicFiniteAutomaton();
        var recognizedTheWord = deterministicAutomaton.Execute(word);

        if (recognizedTheWord)
        {
            _logger.LogInformation("Reconheceu a palavra informada!");
        }
        else
        {
            _logger.LogInformation("Não reconheceu a palavra informada!");
        }
    }
}

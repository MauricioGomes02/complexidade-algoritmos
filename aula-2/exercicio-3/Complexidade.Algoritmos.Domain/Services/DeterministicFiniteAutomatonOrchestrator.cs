using Complexidade.Algoritmo.Domain.EventArgs;
using Complexidade.Algoritmos.Domain.EventArgs;
using Complexidade.Algoritmos.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Complexidade.Algoritmos.Domain.Services;

public class DeterministicFiniteAutomatonOrchestrator : IDeterministicFiniteAutomatonOrchestrator
{
    private readonly ILogger<DeterministicFiniteAutomatonOrchestrator> _logger;
    private readonly IFileManagementService _fileManagementService;
    private readonly IParser _parser;

    public DeterministicFiniteAutomatonOrchestrator(
        ILogger<DeterministicFiniteAutomatonOrchestrator> logger,
        IFileManagementService fileManagementService,
        IParser lexer)
    {
        _logger = logger;

        _fileManagementService = fileManagementService;
        _parser = lexer;

        _fileManagementService.OnReadLine += OnReadLine!;
    }

    public void Execute(string path, IEnumerable<string> word)
    {
        _fileManagementService.Read(path);
        var automaton = _parser.BuildAutomaton();
        automaton.OnPerformTransition += OnPerformTransition!;
        var recognizedTheWord = automaton.Execute(word);

        if (recognizedTheWord)
        {
            _logger.LogInformation("Reconheceu a palavra informada!");
        }
        else
        {
            _logger.LogInformation("Não reconheceu a palavra informada!");
        }
    }

    private void OnReadLine(object sender, ReadFileLineEventArgs eventArgs)
    {
        _parser.Execute(eventArgs.LineNumber, eventArgs.Content);
    }

    private void OnPerformTransition(object sender, StateTransitionEventArgs eventArgs)
    {
        _logger.LogInformation(
            "Ocorreu uma transição do estado '{previousState}' para o estado '{currentState}' ao receber a entrada '{symbol}'",
            eventArgs.PreviousState.Name,
            eventArgs.CurrentState.Name,
            eventArgs.Symbol);
    }
}

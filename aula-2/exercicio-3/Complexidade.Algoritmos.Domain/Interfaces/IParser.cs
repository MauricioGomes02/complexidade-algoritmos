using Complexidade.Algoritmos.Domain.Entities;

namespace Complexidade.Algoritmos.Domain.Interfaces;

public interface IParser
{
    void Execute(int lineNumber, string? content);
    DeterministicFiniteAutomaton BuildAutomaton();
}

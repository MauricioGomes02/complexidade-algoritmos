using Complexidade.Algoritmos.Domain.Entities;

namespace Complexidade.Algoritmos.Domain.Interfaces;

public interface IParser
{
    NonDeterministicFiniteAutomaton Execute(IReadOnlyCollection<Token> tokens);
}

namespace Complexidade.Algoritmos.Domain.Interfaces;

public interface IDeterministicFiniteAutomatonOrchestrator
{
    void Execute(string path, IEnumerable<string> word);
}

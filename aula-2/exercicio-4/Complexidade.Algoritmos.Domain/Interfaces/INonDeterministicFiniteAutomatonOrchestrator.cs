namespace Complexidade.Algoritmos.Domain.Interfaces;

public interface INonDeterministicFiniteAutomatonOrchestrator
{
    void Execute(string path, IEnumerable<string> word);
}

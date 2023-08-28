namespace Complexidade.Algoritmos.Domain.Entities;

public struct TransitionFunctionKey
{
    public TransitionFunctionKey(string state, string symbol)
    {
        State = state;
        Symbol = symbol;
    }

    public string State { get; private set; }
    public string Symbol { get; private set; }
}

namespace Complexidade.Algoritmos.Domain.Entities;

public struct TransitionFunctionKey
{
    public TransitionFunctionKey(State state, string symbol)
    {
        State = state;
        Symbol = symbol;
    }

    public State State { get; private set; }
    public string Symbol { get; private set; }
}

using Complexidade.Algoritmos.Domain.Entities;

namespace Complexidade.Algoritmos.Domain.EventArgs;

public class StateTransitionEventArgs : System.EventArgs
{
    public State PreviousState { get; set; }
    public string Symbol { get; set; }
    public State CurrentState { get; set; }
}

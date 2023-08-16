using Complexidade.Algoritmos.Domain.EventArgs;

namespace Complexidade.Algoritmos.Domain.Entities;

public class DeterministicFiniteAutomaton
{
    private State _currentState;
    private readonly IEnumerable<string> _alphabetSymbols;
    private readonly IEnumerable<State> _states;
    private readonly IEnumerable<State> _endStates;
    private readonly IDictionary<TransitionFunctionKey, State> _transitionFunctions;

    public DeterministicFiniteAutomaton(
        State initialState,
        IEnumerable<string> alphabetSymbols,
        IEnumerable<State> states,
        IEnumerable<State> endStates,
        IDictionary<TransitionFunctionKey, State> transitionFunctions)
    {
        _currentState = initialState;

        _alphabetSymbols = alphabetSymbols;

        _states = states;

        _endStates = endStates;

        _transitionFunctions = transitionFunctions;
    }

    public bool Execute(IEnumerable<string> word)
    {
        foreach (var symbol in word)
        {
            if (!_alphabetSymbols.Contains(symbol))
            {
                return false;
            }

            var transactionFunction = new TransitionFunctionKey(_currentState, symbol);
            if (!_transitionFunctions.TryGetValue(transactionFunction, out var state))
            {
                return false;
            }

            if (!_states.Any(x => x.Name == state.Name))
            {
                return false;
            }

            var previousState = _currentState;
            _currentState = state;

            if (OnPerformTransition is not null)
            {
                var eventArgs = new StateTransitionEventArgs
                {
                    PreviousState = previousState,
                    CurrentState = _currentState,
                    Symbol = symbol
                };
                OnPerformTransition(this, eventArgs);
            }
        }

        return _endStates.Any(x => x.Name == _currentState.Name);
    }

    public event EventHandler<StateTransitionEventArgs>? OnPerformTransition;
}

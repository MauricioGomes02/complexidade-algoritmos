namespace Complexidade.Algoritmos.Domain.Entities;

public class DeterministicFiniteAutomaton
{
    private string _currentState;
    private readonly IEnumerable<string> _alphabetSymbols;
    private readonly IEnumerable<string> _states;
    private readonly IEnumerable<string> _endStates;
    private readonly IDictionary<TransitionFunctionKey, string> _transitionFunctions;

    public DeterministicFiniteAutomaton(
        string initialState,
        IEnumerable<string> alphabetSymbols,
        IEnumerable<string> states,
        IEnumerable<string> endStates,
        IDictionary<TransitionFunctionKey, string> transitionFunctions)
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

            if (!_states.Contains(state))
            {
                return false;
            }

            _currentState = state;
        }

        return _endStates.Any(x => x == _currentState);
    }
}

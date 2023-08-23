namespace Complexidade.Algoritmos.Domain.Entities;

public class NonDeterministicFiniteAutomaton
{
    private IEnumerable<string> _currentStates;
    private readonly IEnumerable<string> _alphabetSymbols;
    private readonly IEnumerable<string> _states;
    private readonly IEnumerable<string> _endStates;
    private readonly IDictionary<TransitionFunctionKey, IEnumerable<string>> _transitionFunctions;

    public NonDeterministicFiniteAutomaton(
        string initialState,
        IEnumerable<string> alphabetSymbols,
        IEnumerable<string> states,
        IEnumerable<string> endStates,
        IDictionary<TransitionFunctionKey, IEnumerable<string>> transitionFunctions)
    {
        _currentStates = new List<string> { initialState };

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

            var newCurrentStates = new List<string>();

            foreach (var currentState in _currentStates)
            {
                var transactionFunction = new TransitionFunctionKey(currentState, symbol);
                if (!_transitionFunctions.TryGetValue(transactionFunction, out var states))
                {
                    return false;
                }

                foreach (var state in states)
                {
                    var exists = false;
                    foreach (var _state in _states)
                    {
                        if (state == _state)
                        {
                            exists = true;
                            break;
                        }
                    }

                    if (!exists)
                    {
                        return false;
                    }
                }

                foreach (var state in states)
                {
                    newCurrentStates.Add(state);
                }
            }

            _currentStates = newCurrentStates;
        }

        return _endStates.Any(x => _currentStates.Any(y => y == x));
    }
}

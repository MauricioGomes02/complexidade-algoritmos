namespace Complexidade.Algoritmos.Domain.Entities;

public class NonDeterministicFiniteAutomaton
{
    private readonly string _initialState;
    private IEnumerable<string> _currentStates;
    private readonly IEnumerable<string> _alphabetSymbols;
    private readonly IEnumerable<string> _states;
    private readonly IEnumerable<string> _endStates;
    private readonly IDictionary<TransitionFunctionKey, IEnumerable<string>> _transitionFunctions;

    private const string EmptyWord = "&";

    public NonDeterministicFiniteAutomaton(
        string initialState,
        IEnumerable<string> alphabetSymbols,
        IEnumerable<string> states,
        IEnumerable<string> endStates,
        IDictionary<TransitionFunctionKey, IEnumerable<string>> transitionFunctions)
    {
        _initialState = initialState;
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
            if (!_alphabetSymbols.Contains(symbol) && symbol != EmptyWord)
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

    public DeterministicFiniteAutomaton ConvertToDeterministicFiniteAutomaton()
    {
        var table = CreateTable();

        var deterministicAutomatonInitialState = _initialState;
        var deterministicAutomatonAlphabetSymbols = _alphabetSymbols.ToList();
        var deterministicAutomatonStates = new List<string>();
        var deterministicAutomatonEndStates = new List<string>();
        var deterministicAutomatonTransitions = new Dictionary<TransitionFunctionKey, string>();

        var count = 0;
        while (count < table.Lines.Count)
        {
            var line = table.Lines[count];
            foreach (var column in line.Columns)
            {
                var states = column.Values;
                var auxStates = states.ToArray();
                if (states.Count > 1)
                {
                    var newState = CreateNewState(states);
                    deterministicAutomatonStates.Add(newState);

                    if (_endStates.Any(x => auxStates.Contains(x)))
                    {
                        deterministicAutomatonEndStates.Add(newState);
                    }

                    ReplaceOccurrences(table.Lines, states, newState);
                    var newLine = new Line(newState);

                    foreach (var value in auxStates)
                    {
                        Line auxLine = table.Lines.First(x => x.State == value);
                        foreach (var auxColumn in auxLine.Columns)
                        {
                            Column? storedColumn = newLine.Columns.Find(x => x.AlphabetSymbol == auxColumn.AlphabetSymbol);
                            if (storedColumn is not null)
                            {
                                storedColumn.Values.AddRange(auxColumn.Values);
                            }
                            else
                            {
                                var newColumn = new Column(auxColumn.AlphabetSymbol, auxColumn.Values);
                                newLine.Columns.Add(newColumn);
                            }
                        }
                    }

                    table.Lines.Add(newLine);
                }
                else
                {
                    deterministicAutomatonStates.Add(line.State);
                }
            }
            count++;
        }

        foreach (var line in table.Lines)
        {
            foreach (var column in line.Columns)
            {
                var key = new TransitionFunctionKey(line.State, column.AlphabetSymbol);
                deterministicAutomatonTransitions.Add(key, column.Values[0]);
            }
        }

        return new DeterministicFiniteAutomaton(
            deterministicAutomatonInitialState,
            deterministicAutomatonAlphabetSymbols,
            deterministicAutomatonStates,
            deterministicAutomatonEndStates,
            deterministicAutomatonTransitions);
    }

    private Table CreateTable()
    {
        var table = new Table();
        foreach (var state in _states)
        {
            var line = new Line(state);
            foreach (var symbol in _alphabetSymbols)
            {
                var column = new Column(symbol);
                var key = new TransitionFunctionKey(state, symbol);
                if (!_transitionFunctions.TryGetValue(key, out var destinationStates))
                {
                    continue;
                }

                foreach (var destinationState in destinationStates)
                {
                    column.Values.Add(destinationState);
                }

                line.Columns.Add(column);
            }
            table.Lines.Add(line);
        }

        return table;
    }

    private static string CreateNewState(IEnumerable<string> states)
    {
        return string.Join("", states);
    }

    private static void ReplaceOccurrences(IEnumerable<Line> lines, List<string> states, string newState)
    {
        foreach (var line in lines)
        {
            foreach (var column in line.Columns)
            {
                if (column.Values.Count != states.Count)
                {
                    continue;
                }

                var count = 0;

                foreach (var state in states)
                {
                    if (column.Values.Contains(state))
                    {
                        count++;
                    }
                }

                if (count == states.Count)
                {
                    column.Values.Clear();
                    column.Values.Add(newState);
                }
            }
        }
    }

    private sealed class Table
    {
        public Table()
        {
            Lines = new List<Line>();
        }

        public Table(List<Line> lines)
        {
            Lines = lines;
        }

        public List<Line> Lines { get; private set; }
    }

    private sealed class Line
    {
        public Line(string state)
        {
            State = state;
            Columns = new List<Column>();
        }

        public Line(string state, List<Column> columns)
        {
            State = state;
            Columns = columns;
        }

        public string State { get; private set; }
        public List<Column> Columns { get; private set; }
    }

    private sealed class Column
    {
        public Column(string alphabetSymbol)
        {
            AlphabetSymbol = alphabetSymbol;
            Values = new List<string>();
        }

        public Column(string alphabetSymbol, List<string> values)
        {
            AlphabetSymbol = alphabetSymbol;
            Values = values;
        }

        public string AlphabetSymbol { get; private set; }
        public List<string> Values { get; private set; }

        public void UpdateValues(List<string> values)
        {
            Values = values;
        }
    }
}

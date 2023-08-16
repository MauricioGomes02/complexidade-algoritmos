using Complexidade.Algoritmos.Domain.Entities;
using Complexidade.Algoritmos.Domain.Enums;
using Complexidade.Algoritmos.Domain.Interfaces;
using System.Text;

namespace Complexidade.Algoritmos.Domain.Services;
public class Parser : IParser
{
    private readonly List<Token> _tokens;

    public Parser()
    {
        _tokens = new List<Token>();
    }

    public void Execute(int lineNumber, string? content)
    {
        switch (lineNumber)
        {
            case 1:
                ProcessInitialState(content);
                break;
            case 2:
                ProcessAlphabetSymbol(content);
                break;
            case 3:
                ProcessStates(content);
                break;
            case 4:
                ProcessEndStates(content);
                break;
            default:
                ProcessTransitionFunction(content);
                break;
        }
    }

    public DeterministicFiniteAutomaton BuildAutomaton()
    {
        var initialState = ((State)_tokens.First(x => x.EToken == EToken.InitialState).Value!)!;
        var alphabetSymbols = _tokens.Where(x => x.EToken == EToken.AlphabetSymbol).Select(x => (x.Value as string)!);
        var states = _tokens.Where(x => x.EToken == EToken.State).Select(x => (State)x.Value!);
        var endStates = _tokens.Where(x => x.EToken == EToken.EndState).Select(x => (State)x.Value!);
        var transitionFunctions = _tokens
            .Where(x => x.EToken == EToken.TransitionFunction)
            .Select(x => (KeyValuePair<TransitionFunctionKey, State>)x.Value!)
            .ToDictionary(x => x.Key, x => x.Value);

        return new DeterministicFiniteAutomaton(
            initialState, 
            alphabetSymbols,
            states,
            endStates,
            transitionFunctions);
    }

    private void ProcessInitialState(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("O argumento não pode ser vazio ou nulo");
        }

        var stringBuilder = new StringBuilder();

        foreach (char character in content)
        {
            if (stringBuilder.Length > 0 && (character == ' ' || character == '\t' || character == '\n'))
            {
                break;
            }

            if (character == ' ' || character == '\t' || character == '\n')
            {
                continue;
            }


            stringBuilder.Append(character);
        }

        var initialState = stringBuilder.ToString();
        var token = new Token(EToken.InitialState, new State(initialState));
        _tokens.Add(token);
    }

    private void ProcessAlphabetSymbol(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("O argumento não pode ser vazio ou nulo");
        }

        var stringBuilder = new StringBuilder();

        foreach (char character in content)
        {
            if (stringBuilder.Length > 0 && (character == ' ' || character == '\t' || character == '\n'))
            {
                var symbol = stringBuilder.ToString();
                var token = new Token(EToken.AlphabetSymbol, symbol);
                _tokens.Add(token);
                stringBuilder.Clear();

                continue;
            }

            if (character == ' ' || character == '\t' || character == '\n')
            {
                continue;
            }


            stringBuilder.Append(character);
        }

        var endSymbol = stringBuilder.ToString();
        var endToken = new Token(EToken.AlphabetSymbol, endSymbol);
        _tokens.Add(endToken);
    }

    private void ProcessStates(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("O argumento não pode ser vazio ou nulo");
        }

        var stringBuilder = new StringBuilder();

        foreach (char character in content)
        {
            if (stringBuilder.Length > 0 && (character == ' ' || character == '\t' || character == '\n'))
            {
                var state = stringBuilder.ToString();

                if (_tokens.Exists(x => x.EToken == EToken.State && ((State)x.Value!).Name == state))
                {
                    throw new InvalidOperationException();
                }

                var token = new Token(EToken.State, new State(state));
                _tokens.Add(token);
                stringBuilder.Clear();

                continue;
            }

            if (character == ' ' || character == '\t' || character == '\n')
            {
                continue;
            }


            stringBuilder.Append(character);
        }

        var endState = stringBuilder.ToString();
        var endToken = new Token(EToken.State, new State(endState));
        _tokens.Add(endToken);
    }

    private void ProcessEndStates(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("O argumento não pode ser vazio ou nulo");
        }

        var stringBuilder = new StringBuilder();

        foreach (char character in content)
        {
            if (stringBuilder.Length > 0 && (character == ' ' || character == '\t' || character == '\n'))
            {
                var state = stringBuilder.ToString();
                var token = new Token(EToken.EndState, new State(state));
                _tokens.Add(token);
                stringBuilder.Clear();

                continue;
            }

            if (character == ' ' || character == '\t' || character == '\n')
            {
                continue;
            }


            stringBuilder.Append(character);
        }

        var endState = stringBuilder.ToString();
        var endToken = new Token(EToken.EndState, new State(endState));
        _tokens.Add(endToken);
    }

    private void ProcessTransitionFunction(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            if (!_tokens.Exists(x => x.EToken == EToken.TransitionFunction))
            {
                throw new ArgumentException("O argumento não pode ser vazio ou nulo");
            }
            else
            {
                return;
            }
        }

        var stringBuilder = new StringBuilder();
        var count = 1;
        var tokens = new List<Token>();

        foreach (char character in content!)
        {
            if (stringBuilder.Length > 0 && (character == ' ' || character == '\t' || character == '\n'))
            {
                var @object = stringBuilder.ToString();
                Token token;

                if (count > 3)
                {
                    throw new InvalidOperationException();
                }

                if (count >= 1 && count < 3)
                {
                    if (!_tokens.Exists(x => x.EToken == EToken.State && ((State)x.Value!).Name == @object))
                    {
                        throw new InvalidOperationException();
                    }

                    token = _tokens.First(x => x.EToken == EToken.State && ((State)x.Value!).Name == @object);
                    tokens.Add(token);
                }
                else
                {
                    if (!_tokens.Exists(x => x.EToken == EToken.AlphabetSymbol && (string)x.Value == @object))
                    {
                        throw new InvalidOperationException();
                    }

                    token = _tokens.First(x => x.EToken == EToken.State && (x.Value as string)! == @object);
                    tokens.Add(token);
                }

                stringBuilder.Clear();

                count++;
            }

            if (character == ' ' || character == '\t' || character == '\n')
            {
                continue;
            }

            stringBuilder.Append(character);
        }

        if (count > 3)
        {
            throw new InvalidOperationException();
        }

        var endSymbol = stringBuilder.ToString();
        var endToken = new Token(EToken.AlphabetSymbol, endSymbol);
        tokens.Add(endToken);

        State currentState = (State)tokens[0].Value!;
        State destinationState = (State)tokens[1].Value!;
        string symbol = (tokens[2].Value as string)!;

        var transitionFunctionKey = new TransitionFunctionKey(currentState, symbol);
        var transitionFunction = new KeyValuePair<TransitionFunctionKey, State>(
            transitionFunctionKey,
            destinationState);

        var transitionFunctionToken = new Token(EToken.TransitionFunction, transitionFunction);
        _tokens.Add(transitionFunctionToken);
    }
}

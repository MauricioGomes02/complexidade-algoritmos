using Complexidade.Algoritmos.Domain.Entities;
using Complexidade.Algoritmos.Domain.Enums;
using Complexidade.Algoritmos.Domain.Interfaces;

namespace Complexidade.Algoritmos.Domain.Services;
public class Parser : IParser
{
    public NonDeterministicFiniteAutomaton Execute(IReadOnlyCollection<Token> tokens)
    {
        // Automaton -> InitialStateLine A
        // InitialStateLine -> state
        // A -> newLine B
        // B -> AlphabetSymbolsLine C
        // AlphabetSymbolsLine -> symbol SymbolList
        // SymbolList -> symbol SymbolList | &
        // C -> newLine D
        // D -> LineStates E
        // StatesLine -> state StateList
        // StateList -> state StateList | &
        // E -> newLine F
        // F -> EndStatesLine G
        // EndStatesLine -> state StateList
        // G -> newLine TransitionsLine
        // TransitionsLine -> state symbol state TransitionList
        // newList state symbol state TransitionList | &

        return ProcessAutomaton(tokens);
    }

    private NonDeterministicFiniteAutomaton ProcessAutomaton(IReadOnlyCollection<Token> tokens)
    {
        var enumerator = tokens.GetEnumerator();
        var initialState = ProcessInitialStateLine(enumerator);
        var a = ProcessA(enumerator);

        return new NonDeterministicFiniteAutomaton(
            initialState,
            a.B.AlphabetSymbols,
            a.B.C.D.States,
            a.B.C.D.E.F.EndStates,
            a.B.C.D.E.F.G.Transitions);
    }

    private static string ProcessInitialStateLine(IEnumerator<Token> enumerator)
    {
        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException();
        }

        var currentToken = enumerator.Current;
        if (currentToken.EToken != EToken.State)
        {
            throw new InvalidOperationException();
        }

        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException();
        }

        return currentToken.Value;
    }

    private static A ProcessA(IEnumerator<Token> enumerator)
    {
        var currentToken = enumerator.Current;
        if (currentToken.EToken != EToken.NewLine)
        {
            throw new InvalidOperationException();
        }

        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException();
        }

        var b = ProcessB(enumerator);
        var a = new A(b);

        return a;
    }

    private static B ProcessB(IEnumerator<Token> enumerator)
    {
        var currentToken = enumerator.Current;
        if (currentToken.EToken != EToken.AlphabetSymbol)
        {
            throw new InvalidOperationException();
        }

        var alphabetSymbols = ProcessAlphabetSymbolsLine(enumerator);
        var c = ProcessC(enumerator);
        return new B(alphabetSymbols, c);
    }

    private static IReadOnlyCollection<string> ProcessAlphabetSymbolsLine(IEnumerator<Token> enumerator)
    {
        var currentToken = enumerator.Current;
        if (currentToken.EToken != EToken.AlphabetSymbol)
        {
            throw new InvalidOperationException();
        }

        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException();
        }

        var symbolList = ProcessSymbolList(enumerator);
        var response = new List<string> { currentToken.Value };
        response.AddRange(symbolList);

        return response;
    }

    private static IReadOnlyCollection<string> ProcessSymbolList(IEnumerator<Token> enumerator)
    {
        var currentToken = enumerator.Current;
        if (currentToken.EToken != EToken.AlphabetSymbol)
        {
            return new List<string>();
        }

        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException();
        }

        var symbolList = ProcessSymbolList(enumerator);
        var response = new List<string> { currentToken.Value };
        response.AddRange(symbolList);

        return response;
    }

    private static C ProcessC(IEnumerator<Token> enumerator)
    {
        var currentToken = enumerator.Current;
        if (currentToken.EToken != EToken.NewLine)
        {
            throw new InvalidOperationException();
        }

        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException();
        }

        var d = ProcessD(enumerator);
        var c = new C(d);

        return c;
    }

    private static D ProcessD(IEnumerator<Token> enumerator)
    {
        var currentToken = enumerator.Current;
        if (currentToken.EToken != EToken.State)
        {
            throw new InvalidOperationException();
        }

        var states = ProcessStatesLine(enumerator);
        var e = ProcessE(enumerator);
        return new D(states, e);
    }

    private static IReadOnlyCollection<string> ProcessStatesLine(IEnumerator<Token> enumerator)
    {
        var currentToken = enumerator.Current;
        if (currentToken.EToken != EToken.State)
        {
            throw new InvalidOperationException();
        }

        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException();
        }

        var stateList = ProcessStateList(enumerator);
        var response = new List<string> { currentToken.Value };
        response.AddRange(stateList);

        return response;
    }

    private static IReadOnlyCollection<string> ProcessStateList(IEnumerator<Token> enumerator)
    {
        var currentToken = enumerator.Current;
        if (currentToken.EToken != EToken.State)
        {
            return new List<string>();
        }

        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException();
        }

        var stateList = ProcessStateList(enumerator);
        var response = new List<string> { currentToken.Value };
        response.AddRange(stateList);

        return response;
    }

    private static E ProcessE(IEnumerator<Token> enumerator)
    {
        var currentToken = enumerator.Current;
        if (currentToken.EToken != EToken.NewLine)
        {
            throw new InvalidOperationException();
        }

        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException();
        }

        var f = ProcessF(enumerator);
        var e = new E(f);

        return e;
    }

    private static F ProcessF(IEnumerator<Token> enumerator)
    {
        var currentToken = enumerator.Current;
        if (currentToken.EToken != EToken.State)
        {
            throw new InvalidOperationException();
        }

        var endStates = ProcessEndStatesLine(enumerator);
        var g = ProcessG(enumerator);
        return new F(endStates, g);
    }

    private static IReadOnlyCollection<string> ProcessEndStatesLine(IEnumerator<Token> enumerator)
    {
        var currentToken = enumerator.Current;
        if (currentToken.EToken != EToken.State)
        {
            throw new InvalidOperationException();
        }

        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException();
        }

        var stateList = ProcessStateList(enumerator);
        var response = new List<string> { currentToken.Value };
        response.AddRange(stateList);

        return response;
    }

    private static G ProcessG(IEnumerator<Token> enumerator)
    {
        var currentToken = enumerator.Current;
        if (currentToken.EToken != EToken.NewLine)
        {
            throw new InvalidOperationException();
        }

        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException();
        }

        var transitions = ProcessTransitionsLine(enumerator);
        var g = new G(transitions);

        return g;
    }

    private static IDictionary<TransitionFunctionKey, IEnumerable<string>> ProcessTransitionsLine(IEnumerator<Token> enumerator)
    {
        var previousState = enumerator.Current;
        if (previousState.EToken != EToken.State)
        {
            throw new InvalidOperationException();
        }

        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException();
        }

        var destinationState = enumerator.Current;
        if (destinationState.EToken != EToken.State)
        {
            throw new InvalidOperationException();
        }

        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException();
        }

        var symbol = enumerator.Current;
        if (symbol.EToken != EToken.AlphabetSymbol)
        {
            throw new InvalidOperationException();
        }

        var transition = new KeyValuePair<TransitionFunctionKey, string>(
            new TransitionFunctionKey(previousState.Value, symbol.Value),
            destinationState.Value);

        var response = new List<KeyValuePair<TransitionFunctionKey, string>> { transition };

        if (enumerator.MoveNext())
        {
            if (enumerator.Current.EToken != EToken.NewLine)
            {
                throw new InvalidOperationException();
            }

            if (!enumerator.MoveNext())
            {
                throw new InvalidOperationException();
            }

            var transitionList = ProcessTransitionList(enumerator);
            response.AddRange(transitionList);
        }

        var dictionary = new Dictionary<TransitionFunctionKey, IEnumerable<string>>();

        foreach (var element in response.GroupBy(x => x.Key))
        {
            dictionary.Add(element.Key, element.Select(x => x.Value));
        }

        return dictionary;
    }

    private static IReadOnlyCollection<KeyValuePair<TransitionFunctionKey, string>> ProcessTransitionList(IEnumerator<Token> enumerator)
    {
        var previousState = enumerator.Current;
        if (previousState.EToken != EToken.State)
        {
            throw new InvalidOperationException();
        }

        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException();
        }

        var destinationState = enumerator.Current;
        if (destinationState.EToken != EToken.State)
        {
            throw new InvalidOperationException();
        }

        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException();
        }

        var symbol = enumerator.Current;
        if (symbol.EToken != EToken.AlphabetSymbol)
        {
            throw new InvalidOperationException();
        }

        var transition = new KeyValuePair<TransitionFunctionKey, string>(
            new TransitionFunctionKey(previousState.Value, symbol.Value),
            destinationState.Value);

        var response = new List<KeyValuePair<TransitionFunctionKey, string>> { transition };

        if (enumerator.MoveNext())
        {
            if (enumerator.Current.EToken != EToken.NewLine)
            {
                throw new InvalidOperationException();
            }

            if (!enumerator.MoveNext())
            {
                throw new InvalidOperationException();
            }

            var transitionList = ProcessTransitionList(enumerator);
            response.AddRange(transitionList);
        }

        return response;
    }

    private sealed class A
    {
        public A(B b)
        {
            B = b;
        }

        public B B { get; private set; }
    }

    private sealed class B
    {
        public B(IReadOnlyCollection<string> alphabetSymbols, C c)
        {
            AlphabetSymbols = alphabetSymbols;
            C = c;
        }

        public IReadOnlyCollection<string> AlphabetSymbols { get; private set; }
        public C C { get; private set; }
    }

    private sealed class C
    {
        public C(D d)
        {
            D = d;
        }

        public D D { get; private set; }
    }

    private sealed class D
    {
        public D(IReadOnlyCollection<string> states, E e)
        {
            States = states;
            E = e;
        }

        public IReadOnlyCollection<string> States { get; private set; }
        public E E { get; private set; }
    }

    private sealed class E
    {
        public E(F f)
        {
            F = f;
        }

        public F F { get; private set; }
    }

    private sealed class F
    {
        public F(IReadOnlyCollection<string> states, G g)
        {
            EndStates = states;
            G = g;
        }

        public IReadOnlyCollection<string> EndStates { get; private set; }
        public G G { get; private set; }
    }

    private sealed class G
    {
        public G(IDictionary<TransitionFunctionKey, IEnumerable<string>> transitions)
        {
            Transitions = transitions;
        }
        public IDictionary<TransitionFunctionKey, IEnumerable<string>> Transitions { get; private set; }
    }
}

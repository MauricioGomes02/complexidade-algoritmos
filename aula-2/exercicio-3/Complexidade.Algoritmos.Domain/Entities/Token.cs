using Complexidade.Algoritmos.Domain.Enums;

namespace Complexidade.Algoritmos.Domain.Entities;

public struct Token
{
    public Token(EToken eToken, object value)
    {
        EToken = eToken;
        Value = value;
    }

    public EToken EToken { get; private set; }
    public object Value { get; private set; }
}

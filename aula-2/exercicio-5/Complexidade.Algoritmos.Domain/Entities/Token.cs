using Complexidade.Algoritmos.Domain.Enums;

namespace Complexidade.Algoritmos.Domain.Entities;

public struct Token
{
    public Token(EToken eToken, string value)
    {
        EToken = eToken;
        Value = value;
    }

    public EToken EToken { get; private set; }
    public string Value { get; private set; }
}

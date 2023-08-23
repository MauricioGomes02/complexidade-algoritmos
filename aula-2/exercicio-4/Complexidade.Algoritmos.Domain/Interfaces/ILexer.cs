using Complexidade.Algoritmos.Domain.Entities;

namespace Complexidade.Algoritmos.Domain.Interfaces;
public interface ILexer
{
    IReadOnlyCollection<Token> Execute(string path);
}

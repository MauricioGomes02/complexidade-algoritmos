using Complexidade.Algoritmos.Domain.Entities;
using Complexidade.Algoritmos.Domain.Enums;
using Complexidade.Algoritmos.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.RegularExpressions;

namespace Complexidade.Algoritmos.Domain.Services;

public class Lexer : ILexer
{
    private readonly IConfiguration _configuration;

    private const string DefaultStatePrefix = "q";

    public Lexer(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IReadOnlyCollection<Token> Execute(string path)
    {
        var tokens = new List<Token>();

        var statePrefix = GetStatePrefixConfiguration() ?? DefaultStatePrefix;

        using var stream = new StreamReader(path);
        string? line;

        do
        {
            line = stream.ReadLine();

            if (line is null)
            {
                break;
            }

            var stringBuilder = new StringBuilder();

            var count = 1;
            foreach (var character in line)
            {
                if (!(char.IsWhiteSpace(character) || character == '\t'))
                {
                    stringBuilder.Append(character);

                    if (line.Length > count)
                    {
                        count++;
                        continue;
                    }
                }

                var element = stringBuilder.ToString();
                stringBuilder.Clear();
                
                Token token;

                if (element == string.Empty)
                {
                    continue;
                }

                if (Regex.IsMatch(element, @$"^{statePrefix}[0-9]$"))
                {
                    token = new Token(EToken.State, element);
                }
                else
                {
                    token = new Token(EToken.AlphabetSymbol, element);
                }

                tokens.Add(token);
                count++;
            }

            tokens.Add(new Token(EToken.NewLine, string.Empty));
        } while (line is not null);

        tokens.RemoveAt(tokens.Count - 1);
        return tokens;
    }

    private string? GetStatePrefixConfiguration() => _configuration.GetSection("StatePrefix").Value;
}

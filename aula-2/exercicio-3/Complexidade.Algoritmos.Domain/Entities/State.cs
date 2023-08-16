namespace Complexidade.Algoritmos.Domain.Entities;

public struct State
{
    public State(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }
}

namespace Complexidade.Algoritmo.Domain.EventArgs;

public class ReadFileLineEventArgs : System.EventArgs
{
    public int LineNumber {  get; set; }
    public string? Content { get; set; }
}

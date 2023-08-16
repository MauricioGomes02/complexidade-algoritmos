using Complexidade.Algoritmo.Domain.EventArgs;

namespace Complexidade.Algoritmos.Domain.Interfaces;

public interface IFileManagementService
{
    void Read(string path);
    event EventHandler<ReadFileLineEventArgs> OnReadLine;
}

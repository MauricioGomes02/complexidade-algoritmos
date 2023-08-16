using Complexidade.Algoritmo.Domain.EventArgs;
using Complexidade.Algoritmos.Domain.Interfaces;

namespace Complexidade.Algoritmo.Infrastructure.Services;
public class FileManagementService : IFileManagementService
{
    public void Read(string path)
    {
        using var streamReader = new StreamReader(path);

        string? lineContent;
        int lineNumber = 1;
        do
        {
            lineContent = streamReader.ReadLine();
            if (OnReadLine is not null)
            {
                var @event = new ReadFileLineEventArgs
                {
                    LineNumber = lineNumber,
                    Content = lineContent
                };
                OnReadLine(this, @event);
                lineNumber++;
            }
        } while (lineContent is not null);
    }

    public event EventHandler<ReadFileLineEventArgs>? OnReadLine;
}

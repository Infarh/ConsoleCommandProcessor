using System.Threading;
using System.Threading.Tasks;

namespace ConsoleCommandProcessor.Commands.Base
{
    public interface ICommand
    {
        Task RunAsync(string CommandText, CancellationToken Cancel);
    }

    public abstract class Command
        : ICommand
    {
        public abstract Task RunAsync(string CommandText, CancellationToken Cancel);
    }
}
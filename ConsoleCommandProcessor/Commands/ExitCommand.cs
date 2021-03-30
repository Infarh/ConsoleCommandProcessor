using System.Threading;
using System.Threading.Tasks;
using ConsoleCommandProcessor.Commands.Base;
using Microsoft.Extensions.Hosting;

namespace ConsoleCommandProcessor.Commands
{
    internal class ExitCommand : Command
    {
        private readonly IHost _Host;

        public ExitCommand(IHost Host) => _Host = Host;

        public override Task RunAsync(string CommandText, CancellationToken Cancel) => _Host.StopAsync(Cancel);
    }
}
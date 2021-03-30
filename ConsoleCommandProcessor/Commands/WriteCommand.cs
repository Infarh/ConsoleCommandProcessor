using System;
using System.Threading;
using System.Threading.Tasks;

using ConsoleCommandProcessor.Commands.Base;

namespace ConsoleCommandProcessor.Commands
{
    public class WriteCommand : Command
    {
        public override async Task RunAsync(string CommandText, CancellationToken Cancel)
        {
            Cancel.ThrowIfCancellationRequested();
            var index = CommandText.IndexOf(' ');
            if (index < 0) return;
            await Console.Out.WriteLineAsync($"Text: {CommandText[(index + 1)..]}");
        }
    }
}

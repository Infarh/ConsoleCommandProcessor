using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ConsoleCommandProcessor.Commands.Base;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsoleCommandProcessor.Workers
{
    public class UserDialog : BackgroundService
    {
        private readonly IServiceProvider _Services;
        private readonly Dictionary<string, Type> _KnownCommandsTypes = new(StringComparer.OrdinalIgnoreCase);

        public UserDialog(IServiceProvider Services)
        {
            _Services = Services;
            var command_type = typeof(Command);
            foreach (var type in typeof(Program).Assembly.DefinedTypes.Where(t => t.IsAssignableTo(typeof(ICommand)) && !t.IsAbstract))
            {
                var type_name = type.Name;
                if (type_name.EndsWith("Command"))
                    _KnownCommandsTypes[type_name[..^"Command".Length]] = type;
                else
                    _KnownCommandsTypes[type_name] = type;
            }
        }

        protected override async Task ExecuteAsync(CancellationToken Stopping)
        {
            while (true)
            {
                using var scope = _Services.CreateScope();

                Stopping.ThrowIfCancellationRequested();

                Console.Write("> ");
                var str = await Console.In.ReadLineAsync().ConfigureAwait(true);

                if (str is not { Length: > 0 }) continue;

                var separator_index = str.IndexOf(' ');
                var cmd_name = separator_index > 0 ? str[..separator_index] : str;

                if (_KnownCommandsTypes.TryGetValue(cmd_name, out var command_type) && scope.ServiceProvider.GetService(command_type) is Command cmd)
                    await cmd.RunAsync(str, Stopping);
                else
                    await Console.Out.WriteLineAsync($"{str} - неизвестная команда");
            }
        }
    }
}

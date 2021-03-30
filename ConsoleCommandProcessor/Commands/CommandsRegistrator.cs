using System.Linq;
using ConsoleCommandProcessor.Commands.Base;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleCommandProcessor.Commands
{
    public static class CommandsRegistrator
    {
        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            foreach (var command in typeof(Command).Assembly.DefinedTypes.Where(t => t.IsAssignableTo(typeof(ICommand)) && !t.IsAbstract))
                services.AddScoped(command);
            return services;
        }
    }
}

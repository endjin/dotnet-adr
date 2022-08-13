// <copyright file="AdrCli.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli
{
    using System;
    using System.CommandLine.Parsing;
    using System.Text;
    using System.Threading.Tasks;
    using Endjin.Adr.Cli.Configuration;
    using Endjin.Adr.Cli.Infrastructure;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// A CLI tool for creating and manading Architectural Decision Records.
    /// </summary>
    public static class AdrCli
    {
        private static readonly ServiceCollection ServiceCollection = new();

        /// <summary>
        /// Architectural Decision Records .NET Global Tool.
        /// </summary>
        /// <param name="args">Command Line Switches.</param>
        /// <returns>Exit Code.</returns>
        public static async Task<int> Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            ICompositeConsole console = new CompositeConsole();

            return await new CommandLineParser(
                console,
                new FileSystemRoamingProfileAppEnvironment(console),
                ServiceCollection).Create().InvokeAsync(args, console).ConfigureAwait(false);

/*            serviceCollection.ConfigureDependencies();

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            await serviceProvider.GetRequiredService<IAppEnvironmentManager>()
                                 .SetFirstRunDesiredStateAsync()
                                 .ConfigureAwait(false);

            var cmd = new CommandLineBuilder()
                .AddCommand(serviceProvider.GetRequiredService<ICommandFactory<InitCommandFactory>>().Create())
                .AddCommand(serviceProvider.GetRequiredService<ICommandFactory<NewCommandFactory>>().Create())
                .AddCommand(serviceProvider.GetRequiredService<ICommandFactory<TemplatesCommandFactory>>().Create())
                .AddCommand(serviceProvider.GetRequiredService<ICommandFactory<EnvironmentCommandFactory>>().Create())
                .UseDefaults()
                .Build();

            return await cmd.InvokeAsync(args).ConfigureAwait(false);*/
        }
    }
}
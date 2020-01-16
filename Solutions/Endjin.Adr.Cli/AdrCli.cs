// <copyright file="AdrCli.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli
{
    using System.CommandLine.Builder;
    using System.CommandLine.Invocation;
    using System.Threading.Tasks;
    using Endjin.Adr.Cli.Commands;
    using Endjin.Adr.Cli.Commands.Environment;
    using Endjin.Adr.Cli.Commands.Init;
    using Endjin.Adr.Cli.Commands.New;
    using Endjin.Adr.Cli.Commands.Templates;
    using Endjin.Adr.Cli.Configuration.Contracts;
    using Endjin.Adr.Cli.Extensions;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// A CLI tool for creating and manading Architectural Decision Records.
    /// </summary>
    public static class AdrCli
    {
        /// <summary>
        /// Architectural Decision Records .NET Global Tool.
        /// </summary>
        /// <param name="args">Command Line Switches.</param>
        /// <returns>Exit Code.</returns>
        public static async Task<int> Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.ConfigureDependencies();

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            await serviceProvider.GetRequiredService<IAppEnvironmentManager>()
                                 .SetFirstRunDesiredStateAsync()
                                 .ConfigureAwait(false);

            var cmd = new CommandLineBuilder()
                .AddCommand(serviceProvider.GetRequiredService<ICommandFactory<InitCommand>>().Create())
                .AddCommand(serviceProvider.GetRequiredService<ICommandFactory<NewCommand>>().Create())
                .AddCommand(serviceProvider.GetRequiredService<ICommandFactory<TemplatesCommand>>().Create())
                .AddCommand(serviceProvider.GetRequiredService<ICommandFactory<EnvironmentCommand>>().Create())
                .UseDefaults()
                .Build();

            return await cmd.InvokeAsync(args).ConfigureAwait(false);
        }
    }
}
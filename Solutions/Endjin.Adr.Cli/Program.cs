// <copyright file="Program.cs" company="Endjin Limited">
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
    using Endjin.Adr.Cli.Templates;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// A CLI tool for creating and manading Architectural Decision Records.
    /// </summary>
    public static class Program
    {
        private static readonly ServiceCollection ServiceCollection = new();

        /// <summary>
        /// Architectural Decision Records .NET Global Tool.
        /// </summary>
        /// <param name="args">Command Line Switches.</param>
        /// <returns>Exit Code.</returns>
        public static async Task<int> Main(string[] args)
        {
            ICompositeConsole console = new CompositeConsole();

            return await new CommandLineParser(
                console,
                new FileSystemRoamingProfileAppEnvironment(console),
                new AppEnvironmentManager(
                  new FileSystemLocalProfileAppEnvironment(),
                  new NuGetTemplatePackageManager(new FileSystemLocalProfileAppEnvironment()),
                  new TemplateSettingsManager(new FileSystemLocalProfileAppEnvironment()),
                  console),
                new TemplateSettingsManager(new FileSystemLocalProfileAppEnvironment()),
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
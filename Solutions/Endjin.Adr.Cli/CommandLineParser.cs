// <copyright file="CommandLineParser.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli
{
    using System.CommandLine;
    using System.CommandLine.Builder;
    using System.CommandLine.Invocation;
    using System.CommandLine.Parsing;
    using System.Threading.Tasks;

    using Endjin.Adr.Cli.Commands.Init;
    using Endjin.Adr.Cli.Configuration.Contracts;
    using Endjin.Adr.Cli.Infrastructure;
    using Microsoft.Extensions.DependencyInjection;

    public class CommandLineParser
    {
        private readonly ICompositeConsole console;
        private readonly IAppEnvironment appEnvironment;
        private readonly IServiceCollection services;

        public CommandLineParser(ICompositeConsole console, IAppEnvironment appEnvironment, IServiceCollection services)
        {
            this.console = console;
            this.services = services;
            this.appEnvironment = appEnvironment;
        }

        public delegate Task EnvironmentInit(IServiceCollection services, EnvironmentOptions options, ICompositeConsole console, IAppEnvironment appEnvironment, InvocationContext invocationContext = null);

        public Parser Create(EnvironmentInit environmentInit = null)
        {
            environmentInit ??= EnvironmentInitHandler.ExecuteAsync;

            // Set up intrinsic commands that will always be available.
            RootCommand rootCommand = Root();
            rootCommand.AddCommand(Init());

            var commandBuilder = new CommandLineBuilder(rootCommand);

            return commandBuilder.UseDefaults().Build();

            static RootCommand Root()
            {
                return new RootCommand
                {
                    Name = "adr",
                    Description = "Architectural Decision Records for .NET",
                };
            }

            Command Init()
            {
                var cmd = new Command("init", "Initialises a new ADR repository.")
                {
                    Handler = CommandHandler.Create<EnvironmentOptions, InvocationContext>(async (options, context) =>
                    {
                        await environmentInit(this.services, options, this.console, this.appEnvironment, context).ConfigureAwait(false);
                    }),

                    /*Handler = CommandHandler.Create((string path) =>
                    {
                        if (string.IsNullOrEmpty(path))
                        {
                            path = Path.Combine(Directory.GetCurrentDirectory(), "docs", "adr");
                        }

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);

                            Console.WriteLine($"Created ADR Repository in '{path}'");
                        }
                        else
                        {
                            Console.WriteLine($"'{path}' already exists.");
                        }
                    }),*/
                };

                cmd.AddArgument(new Argument<string>("path") { Arity = ArgumentArity.ZeroOrOne });

                return cmd;
            }
        }
    }
}
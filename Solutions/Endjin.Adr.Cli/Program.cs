// <copyright file="Program.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Endjin.Adr.Cli.Commands.Init;
using Endjin.Adr.Cli.Commands.New;
using Endjin.Adr.Cli.Commands.Templates.Default;
using Endjin.Adr.Cli.Commands.Templates.List;
using Endjin.Adr.Cli.Commands.Templates.Package;
using Endjin.Adr.Cli.Commands.Templates.Update;
using Endjin.Adr.Cli.Extensions;
using Endjin.Adr.Cli.Infrastructure.Injection;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Endjin.Adr.Cli;

/// <summary>
/// A CLI tool for creating and managing Architectural Decision Records.
/// </summary>
public static class Program
{
    /// <summary>
    /// Architectural Decision Records .NET Global Tool.
    /// </summary>
    /// <param name="args">Command Line Switches.</param>
    /// <returns>Exit Code.</returns>
    public static Task<int> Main(string[] args)
    {
        var registrations = new ServiceCollection();
        registrations.ConfigureDependencies();

        var registrar = new TypeRegistrar(registrations);
        var app = new CommandApp(registrar);
        app.Configure(config =>
        {
            config.CaseSensitivity(CaseSensitivity.None);
            config.SetApplicationName("adr");
            config.AddExample(new[] { "new", "\"Integration of an Event Store\"" });
            config.AddExample(new[] { "new", "-i", "1", "\"Integration of an Event Store\"" });
            config.AddExample(new[] { "environment", "init" });
            config.AddExample(new[] { "environment", "reset" });
            config.AddExample(new[] { "templates", "set" });
            config.AddExample(new[] { "templates", "list" });
            config.AddExample(new[] { "templates", "package", "set" });
            config.AddExample(new[] { "templates", "package", "show" });
            config.AddExample(new[] { "templates", "package", "update" });

            config.AddCommand<NewAdrCommand>("new")
                  .WithDescription("Creates a new Architectural Decision Record, from the default ADR Template.");

            config.AddBranch("environment", environment =>
            {
                environment.AddCommand<EnvironmentInitCommand>("init")
                           .WithDescription("Initialises a new ADR repository.");
            });

            config.AddBranch("templates", templates =>
            {
                templates.SetDescription("set, list, ADR templates");
                templates.AddCommand<TemplatesSetCommand>("set");
                templates.AddCommand<TemplatesListCommand>("list");

                templates.AddBranch("package", package =>
                {
                    package.SetDescription("Show, set, update, template packages.");
                    package.AddCommand<TemplatesPackageSetCommand>("set");
                    package.AddCommand<TemplatesPackageShowCommand>("show");
                    package.AddCommand<TemplatesUpdateCommand>("update");
                });
            });
#if DEBUG
            config.PropagateExceptions();
#endif
            config.ValidateExamples();
        });

        return app.RunAsync(args);
    }
}
// <copyright file="Program.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.IO;
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

using Spectre.Console;
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
    ServiceCollection registrations = new();
    registrations.ConfigureDependencies();

    TypeRegistrar registrar = new(registrations);
    CommandApp app = new(registrar);

    app.Configure(config =>
    {
        config.Settings.PropagateExceptions = false;
        config.CaseSensitivity(CaseSensitivity.None);
        config.SetApplicationName("adr");
        config.AddExample("new", "\"Integration of an Event Store\"");
        config.AddExample("new", "-i", "1", "\"Integration of an Event Store\"");
        config.AddExample("environment", "init");

        // config.AddExample(new[] { "environment", "reset" });
        config.AddExample("templates", "set");
        config.AddExample("templates", "list");
        config.AddExample("templates", "package", "set");
        config.AddExample("templates", "package", "show");
        config.AddExample("templates", "package", "update");

        config.AddCommand<NewAdrCommand>("new")
              .WithDescription("Creates a new Architectural Decision Record, from the default ADR Template.");

        config.AddBranch("environment", environment =>
        {
            environment.SetDescription("Initialize and reset the local environment");
            environment.AddCommand<EnvironmentInitCommand>("init")
                       .WithDescription("Initialises a new ADR repository.");
        });

        config.AddBranch("templates", templates =>
        {
            templates.SetDescription("set, list, ADR templates");
            templates.AddCommand<TemplatesSetCommand>("set").WithDescription("Set the active ADR template");
            templates.AddCommand<TemplatesListCommand>("list").WithDescription("List the installed ADR templates");

            templates.AddBranch("package", package =>
            {
                package.SetDescription("Show, set, update, template packages");
                package.AddCommand<TemplatesPackageSetCommand>("set").WithDescription("set the active ADR template package");
                package.AddCommand<TemplatesPackageShowCommand>("show").WithDescription("show all available ADR template packages");
                package.AddCommand<TemplatesUpdateCommand>("update").WithDescription("update the specified ADR template package");
            });
        });
/*#if DEBUG
        config.PropagateExceptions();
#endif*/
        config.ValidateExamples();
    });

    return app.RunAsync(args);
}
}
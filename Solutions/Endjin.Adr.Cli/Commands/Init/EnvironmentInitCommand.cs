// <copyright file="EnvironmentInitCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

using Endjin.Adr.Cli.Abstractions;

using Spectre.Console;
using Spectre.Console.Cli;

namespace Endjin.Adr.Cli.Commands.Init;

public class EnvironmentInitCommand : AsyncCommand<EnvironmentInitCommand.Settings>
{
    /// <inheritdoc/>
    public override Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        AnsiConsole.Write(new FigletText("dotnet-adr").Color(Color.Green));

        if (string.IsNullOrEmpty(settings.Path))
        {
            settings.Path = Path.Combine(Directory.GetCurrentDirectory(), "docs", "adr");
        }

        if (!Directory.Exists(settings.Path))
        {
            Directory.CreateDirectory(settings.Path);

            AnsiConsole.MarkupLine($"Created ADR Repository in [aqua]'{settings.Path}'[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"'[red]{settings.Path}'[/] already exists.");
        }

        return Task.FromResult(ReturnCodes.Ok);
    }

    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[Path]")]
        public string Path { get; set; }
    }
}
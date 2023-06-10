// <copyright file="EnvironmentResetCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Endjin.Adr.Cli.Abstractions;
using Endjin.Adr.Cli.Configuration.Contracts;

using Spectre.Console;
using Spectre.Console.Cli;

namespace Endjin.Adr.Cli.Commands.Init;

public class EnvironmentResetCommand : AsyncCommand
{
    private readonly IAppEnvironment appEnvironment;

    public EnvironmentResetCommand(IAppEnvironment appEnvironment)
    {
        this.appEnvironment = appEnvironment;
    }

    /// <inheritdoc/>
    public override async Task<int> ExecuteAsync([NotNull] CommandContext context)
    {
        AnsiConsole.Write(new FigletText("dotnet-adr").Color(Color.Green));
        AnsiConsole.MarkupLine($"Resetting the local environment.");

        await this.appEnvironment.InitializeAsync().ConfigureAwait(false);

        AnsiConsole.MarkupLine($"[green]Done![/]");

        return ReturnCodes.Ok;
    }
}
// <copyright file="TemplatesPackageShowCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Endjin.Adr.Cli.Abstractions;
using Endjin.Adr.Cli.Configuration;
using Endjin.Adr.Cli.Configuration.Contracts;

using Spectre.Console;
using Spectre.Console.Cli;

namespace Endjin.Adr.Cli.Commands.Templates.Package;

public class TemplatesPackageShowCommand : AsyncCommand
{
    private readonly ITemplateSettingsManager templateSettingsManager;

    public TemplatesPackageShowCommand(ITemplateSettingsManager templateSettingsManager)
    {
        this.templateSettingsManager = templateSettingsManager;
    }

    /// <inheritdoc/>
    public override Task<int> ExecuteAsync([NotNull] CommandContext context)
    {
        AnsiConsole.Write(new FigletText("dotnet-adr").Color(Color.Green));

        TemplateSettings templateSettings = this.templateSettingsManager.LoadSettings(nameof(TemplateSettings));

        AnsiConsole.MarkupLine($"NuGet ADR Template Package: [aqua]{templateSettings.DefaultTemplatePackage}[/]");

        return Task.FromResult(ReturnCodes.Ok);
    }
}
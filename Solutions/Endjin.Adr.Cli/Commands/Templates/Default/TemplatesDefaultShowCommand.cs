// <copyright file="TemplatesDefaultShowCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;

using Endjin.Adr.Cli.Abstractions;
using Endjin.Adr.Cli.Configuration;
using Endjin.Adr.Cli.Configuration.Contracts;
using Endjin.Adr.Cli.Templates;

using Spectre.Console;
using Spectre.Console.Cli;

namespace Endjin.Adr.Cli.Commands.Templates.Default;

public class TemplatesDefaultShowCommand : AsyncCommand
{
    private readonly ITemplateSettingsManager templateSettingsManager;

    public TemplatesDefaultShowCommand(ITemplateSettingsManager templateSettingsManager)
    {
        this.templateSettingsManager = templateSettingsManager;
    }

    /// <inheritdoc/>
    public override Task<int> ExecuteAsync([NotNull] CommandContext context)
    {
        AnsiConsole.Write(new FigletText("dotnet-adr").Color(Color.Green));

        TemplateSettings templateSettings = this.templateSettingsManager.LoadSettings(nameof(TemplateSettings));
        TemplatePackageDetail template = templateSettings.MetaData.Details.Find(x => x.FullPath == templateSettings.DefaultTemplate);

        AnsiConsole.MarkupLine($"[aqua]Title:[/] {template.Title}");
        AnsiConsole.MarkupLine($"[aqua]Id:[/] {template.Id}");
        AnsiConsole.MarkupLine($"[aqua]Description:[/] {template.Description}");
        AnsiConsole.MarkupLine($"[aqua]Authors:[/] {template.Authors}");
        AnsiConsole.MarkupLine($"[aqua]Effort:[/] {template.Effort}");
        AnsiConsole.MarkupLine($"[aqua]More Info:[/] {template.MoreInfo}");
        AnsiConsole.MarkupLine($"[aqua]Last Modified:[/] {template.LastModified.ToString(CultureInfo.InvariantCulture)}");
        AnsiConsole.MarkupLine($"[aqua]Version:[/] {template.Version}");

        return Task.FromResult(ReturnCodes.Ok);
    }
}
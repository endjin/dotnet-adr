// <copyright file="TemplatesPackageUpdateCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Endjin.Adr.Cli.Abstractions;
using Endjin.Adr.Cli.Configuration;
using Endjin.Adr.Cli.Configuration.Contracts;
using Endjin.Adr.Cli.Templates;

using Spectre.Console;
using Spectre.Console.Cli;

namespace Endjin.Adr.Cli.Commands.Templates.Package;

public class TemplatesPackageUpdateCommand : AsyncCommand
{
    private readonly ITemplateSettingsManager templateSettingsManager;
    private readonly ITemplatePackageManager templatePackageManager;

    public TemplatesPackageUpdateCommand(ITemplateSettingsManager templateSettingsManager, ITemplatePackageManager templatePackageManager)
    {
        this.templateSettingsManager = templateSettingsManager;
        this.templatePackageManager = templatePackageManager;
    }

    /// <inheritdoc/>
    public override async Task<int> ExecuteAsync([NotNull] CommandContext context)
    {
        AnsiConsole.Write(new FigletText("dotnet-adr").Color(Color.Green));

        TemplateSettings currentSettings = this.templateSettingsManager.LoadSettings(nameof(TemplateSettings));

        AnsiConsole.MarkupLine($"Current ADR Templates version [aqua]{currentSettings.MetaData.Version}[/]");

        TemplatePackageMetaData templateMetaData = await this.templatePackageManager.InstallLatestAsync(currentSettings.DefaultTemplatePackage).ConfigureAwait(false);
        TemplatePackageDetail defaultTemplate = templateMetaData.Details.Find(x => x.IsDefault) ?? templateMetaData.Details[0];

        currentSettings.MetaData = templateMetaData;
        currentSettings.DefaultTemplate = defaultTemplate.FullPath;

        AnsiConsole.MarkupLine($"Downloaded ADR Templates [yellow]{currentSettings.DefaultTemplatePackage}[/] version [aqua]{templateMetaData.Version}[/]");

        this.templateSettingsManager.SaveSettings(currentSettings, nameof(TemplateSettings));

        AnsiConsole.MarkupLine($"Installed ADR Templates [yellow]{currentSettings.DefaultTemplatePackage}[/] version [aqua]{templateMetaData.Version}[/]");

        return ReturnCodes.Ok;
    }
}
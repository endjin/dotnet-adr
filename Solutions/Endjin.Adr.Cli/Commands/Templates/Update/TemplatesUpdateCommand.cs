// <copyright file="TemplatesUpdateCommand.cs" company="Endjin Limited">
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

namespace Endjin.Adr.Cli.Commands.Templates.Update;
public class TemplatesUpdateCommand : AsyncCommand<TemplatesUpdateCommand.Settings>
{
    private readonly ITemplateSettingsManager templateSettingsManager;
    private readonly ITemplatePackageManager templatePackageManager;

    public TemplatesUpdateCommand(ITemplateSettingsManager templateSettingsManager, ITemplatePackageManager templatePackageManager)
    {
        this.templateSettingsManager = templateSettingsManager;
        this.templatePackageManager = templatePackageManager;
    }

    /// <inheritdoc/>
    public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        TemplateSettings currentSettings = this.templateSettingsManager.LoadSettings(nameof(TemplateSettings));

        AnsiConsole.MarkupLine($"Current ADR Templates version {currentSettings.MetaData.Version}");

        TemplatePackageMetaData templateMetaData = await this.templatePackageManager.InstallLatestAsync(currentSettings.DefaultTemplatePackage).ConfigureAwait(false);
        TemplatePackageDetail defaultTemplate = templateMetaData.Details.Find(x => x.IsDefault) ?? templateMetaData.Details[0];

        currentSettings.MetaData = templateMetaData;
        currentSettings.DefaultTemplate = defaultTemplate.FullPath;

        AnsiConsole.MarkupLine($"Downloaded ADR Templates version {templateMetaData.Version}");

        this.templateSettingsManager.SaveSettings(currentSettings, nameof(TemplateSettings));

        AnsiConsole.MarkupLine($"Updated ADR Templates to version {templateMetaData.Version}");

        return ReturnCodes.Ok;
    }

    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[Title]")]
        public string Title { get; set; }

        [CommandArgument(0, "[Title]")]
        public int? Id { get; set; }
    }
}
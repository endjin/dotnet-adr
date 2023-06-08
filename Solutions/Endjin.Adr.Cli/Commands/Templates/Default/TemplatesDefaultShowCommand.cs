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

public class TemplatesDefaultShowCommand : AsyncCommand<TemplatesDefaultShowCommand.Settings>
{
    private readonly ITemplateSettingsManager templateSettingsManager;

    public TemplatesDefaultShowCommand(ITemplateSettingsManager templateSettingsManager)
    {
        this.templateSettingsManager = templateSettingsManager;
    }

    /// <inheritdoc/>
    public override Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        TemplateSettings templateSettings = this.templateSettingsManager.LoadSettings(nameof(TemplateSettings));
        TemplatePackageDetail template = templateSettings.MetaData.Details.Find(x => x.FullPath == templateSettings.DefaultTemplate);

        AnsiConsole.MarkupLine($"Title: {template.Title}");
        AnsiConsole.MarkupLine($"Id: {template.Id}");
        AnsiConsole.MarkupLine($"Description: {template.Description}");
        AnsiConsole.MarkupLine($"Authors: {template.Authors}");
        AnsiConsole.MarkupLine($"Effort: {template.Effort}");
        AnsiConsole.MarkupLine($"More Info: {template.MoreInfo}");
        AnsiConsole.MarkupLine($"Last Modified: {template.LastModified.ToString(CultureInfo.InvariantCulture)}");
        AnsiConsole.MarkupLine($"Version: {template.Version}");

        return Task.FromResult(ReturnCodes.Ok);
    }

    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[Title]")]
        public string Title { get; set; }

        [CommandArgument(0, "[Title]")]
        public int? Id { get; set; }
    }
}
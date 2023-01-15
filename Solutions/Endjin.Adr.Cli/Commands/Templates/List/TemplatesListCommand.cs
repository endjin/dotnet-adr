// <copyright file="TemplatesListCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;
using Endjin.Adr.Cli.Abstractions;
using Endjin.Adr.Cli.Configuration;
using Endjin.Adr.Cli.Configuration.Contracts;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Endjin.Adr.Cli.Commands.Templates.List;
public class TemplatesListCommand : AsyncCommand<TemplatesListCommand.Settings>
{
    private readonly ITemplateSettingsManager templateSettingsManager;

    public TemplatesListCommand(ITemplateSettingsManager templateSettingsManager)
    {
        this.templateSettingsManager = templateSettingsManager;
    }

    /// <inheritdoc/>
    public override Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        var templateSettings = this.templateSettingsManager.LoadSettings(nameof(TemplateSettings));

        if (settings.IdsOnly)
        {
            foreach (var template in templateSettings.MetaData.Details)
            {
                AnsiConsole.MarkupLine(template.Id);
            }
        }
        else
        {
            AnsiConsole.MarkupLine("-------");

            foreach (var template in templateSettings.MetaData.Details)
            {
                AnsiConsole.MarkupLine(string.Empty);
                AnsiConsole.MarkupLine($"Title: {template.Title}");
                AnsiConsole.MarkupLine($"Id: {template.Id}");
                AnsiConsole.MarkupLine($"Description: {template.Description}");
                AnsiConsole.MarkupLine($"Authors: {template.Authors}");
                AnsiConsole.MarkupLine($"Effort: {template.Effort}");
                AnsiConsole.MarkupLine($"More Info: {template.MoreInfo}");
                AnsiConsole.MarkupLine($"Last Modified: {template.LastModified.ToString(CultureInfo.InvariantCulture)}");
                AnsiConsole.MarkupLine($"Version: {template.Version}");
                AnsiConsole.MarkupLine(string.Empty);
                AnsiConsole.MarkupLine("-------");
            }
        }

        return Task.FromResult(ReturnCodes.Ok);
    }

    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[IdOnly]")]
        public bool IdsOnly { get; set; }
    }
}
// <copyright file="TemplatesSetCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Endjin.Adr.Cli.Abstractions;
using Endjin.Adr.Cli.Configuration;
using Endjin.Adr.Cli.Configuration.Contracts;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Endjin.Adr.Cli.Commands.Templates.Default;
public class TemplatesSetCommand : AsyncCommand<TemplatesSetCommand.Settings>
{
    private readonly ITemplateSettingsManager templateSettingsManager;

    public TemplatesSetCommand(ITemplateSettingsManager templateSettingsManager)
    {
        this.templateSettingsManager = templateSettingsManager;
    }

    /// <inheritdoc/>
    public override Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        if (!string.IsNullOrEmpty(settings.TemplateId))
        {
            var templateSettings = this.templateSettingsManager.LoadSettings(nameof(TemplateSettings));
            var template = templateSettings.MetaData.Details.Find(x => x.Id == settings.TemplateId);

            templateSettings.DefaultTemplate = template.FullPath;

            this.templateSettingsManager.SaveSettings(templateSettings, nameof(TemplateSettings));

            AnsiConsole.MarkupLine($"Setting \"{template.Title}\" as the default ADR template.");

            return Task.FromResult(ReturnCodes.Ok);
        }

        return Task.FromResult(ReturnCodes.Error);
    }

    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[Title]")]
        public string TemplateId { get; set; }

        [CommandArgument(0, "[Title]")]
        public int? Id { get; set; }
    }
}
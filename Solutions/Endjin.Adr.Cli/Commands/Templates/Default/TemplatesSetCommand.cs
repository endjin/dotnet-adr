// <copyright file="TemplatesSetCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Endjin.Adr.Cli.Abstractions;
using Endjin.Adr.Cli.Configuration;
using Endjin.Adr.Cli.Configuration.Contracts;
using Endjin.Adr.Cli.Templates;

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
        TemplateSettings templateSettings = this.templateSettingsManager.LoadSettings(nameof(TemplateSettings));
        TemplatePackageDetail template = templateSettings.MetaData.Details.Find(x => x.Id == settings.TemplateId);

        templateSettings.DefaultTemplate = template.FullPath;

        this.templateSettingsManager.SaveSettings(templateSettings, nameof(TemplateSettings));

        AnsiConsole.MarkupLine($"Setting \"{template.Title}\" as the default ADR template.");

        return Task.FromResult(ReturnCodes.Ok);
    }

    public class Settings : CommandSettings
    {
        [Description("The ADR Template Id. Use template list --id-only to list available templates.")]
        [CommandArgument(0, "[TemplateId]")]
        public string TemplateId { get; set; }

        public override ValidationResult Validate()
        {
            return string.IsNullOrEmpty(this.TemplateId)
                ? ValidationResult.Error("Please specify the TemplateId. Use template list --id-only to list available templates.")
                : ValidationResult.Success();
        }
    }
}
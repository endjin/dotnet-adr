// <copyright file="TemplatesListCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;

using Endjin.Adr.Cli.Abstractions;
using Endjin.Adr.Cli.Configuration;
using Endjin.Adr.Cli.Configuration.Contracts;
using Endjin.Adr.Cli.Templates;

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
        AnsiConsole.Write(new FigletText("dotnet-adr").Color(Color.Green));

        TemplateSettings templateSettings = this.templateSettingsManager.LoadSettings(nameof(TemplateSettings));

        if (settings.IdsOnly)
        {
            foreach (TemplatePackageDetail template in templateSettings.MetaData.Details)
            {
                AnsiConsole.MarkupLine(template.Id);
            }
        }
        else
        {
            if (settings.FormatAsList)
            {
                AnsiConsole.MarkupLine("[green]-------[/]");

                foreach (TemplatePackageDetail template in templateSettings.MetaData.Details)
                {
                    AnsiConsole.MarkupLine(string.Empty);
                    AnsiConsole.MarkupLine($"[aqua]Title:[/] {template.Title}");
                    AnsiConsole.MarkupLine($"[aqua]Id:[/] {template.Id}");
                    AnsiConsole.MarkupLine($"[aqua]Description:[/] {template.Description}");
                    AnsiConsole.MarkupLine($"[aqua]Authors:[/] {template.Authors}");
                    AnsiConsole.MarkupLine($"[aqua]Effort:[/] {template.Effort}");
                    AnsiConsole.MarkupLine($"[aqua]More Info:[/] {template.MoreInfo}");
                    AnsiConsole.MarkupLine($"[aqua]Last Modified:[/] {template.LastModified.ToString(CultureInfo.InvariantCulture)}");
                    AnsiConsole.MarkupLine($"[aqua]Version:[/] {template.Version}");
                    AnsiConsole.MarkupLine(string.Empty);
                    AnsiConsole.MarkupLine("[green]-------[/]");
                }
            }
            else
            {
                var table = new Table();
                table.AddColumn("Id");
                table.AddColumn("Title");
                table.AddColumn("Description");
                table.AddColumn("Authors");
                table.AddColumn("Effort");
                table.AddColumn("More Info");
                table.AddColumn("Last Modified");
                table.AddColumn("Version");

                foreach (TemplatePackageDetail template in templateSettings.MetaData.Details)
                {
                    table.AddRow(
                        template.Id,
                        template.Title,
                        template.Description,
                        template.Authors,
                        template.Effort,
                        template.MoreInfo,
                        template.LastModified.ToString(CultureInfo.InvariantCulture),
                        template.Version.ToString());
                }

                AnsiConsole.Write(table);
            }
        }

        return Task.FromResult(ReturnCodes.Ok);
    }

    public class Settings : CommandSettings
    {
        [CommandOption("--id-only")]
        [DefaultValue(false)]
        public bool IdsOnly { get; set; }

        [CommandOption("--format-list")]
        [DefaultValue(false)]
        public bool FormatAsList { get; set; }
    }
}
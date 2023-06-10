﻿// <copyright file="TemplatesPackageSetCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Endjin.Adr.Cli.Abstractions;
using Endjin.Adr.Cli.Configuration;
using Endjin.Adr.Cli.Configuration.Contracts;

using Spectre.Console;
using Spectre.Console.Cli;

namespace Endjin.Adr.Cli.Commands.Templates.Package;

public class TemplatesPackageSetCommand : AsyncCommand<TemplatesPackageSetCommand.Settings>
{
    private readonly ITemplateSettingsManager templateSettingsManager;

    public TemplatesPackageSetCommand(ITemplateSettingsManager templateSettingsManager)
    {
        this.templateSettingsManager = templateSettingsManager;
    }

    /// <inheritdoc/>
    public override Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        AnsiConsole.Write(new FigletText("dotnet-adr").Color(Color.Green));

        if (string.IsNullOrEmpty(settings.PackageId))
        {
            return Task.FromResult(ReturnCodes.Error);
        }

        TemplateSettings templateSettings = this.templateSettingsManager.LoadSettings(nameof(TemplateSettings));
        templateSettings.DefaultTemplatePackage = settings.PackageId;

        this.templateSettingsManager.SaveSettings(templateSettings, nameof(TemplateSettings));

        AnsiConsole.MarkupLine($"""Setting [aqua]"{templateSettings.DefaultTemplatePackage}"[/] as the default NuGet ADR Template package.""");

        return Task.FromResult(ReturnCodes.Ok);
    }

    public override ValidationResult Validate(CommandContext context, Settings settings)
    {
        if (string.IsNullOrEmpty(settings.PackageId))
        {
            return ValidationResult.Error("Please specify the PackageId");
        }

        return base.Validate(context, settings);
    }

    public class Settings : CommandSettings
    {
        [Description("The name to display")]
        [CommandArgument(0, "[PackageId]")]
        public string PackageId { get; set; }

        public override ValidationResult Validate()
        {
            return string.IsNullOrEmpty(this.PackageId)
                ? ValidationResult.Error("Please specify the PackageId")
                : ValidationResult.Success();
        }
    }
}
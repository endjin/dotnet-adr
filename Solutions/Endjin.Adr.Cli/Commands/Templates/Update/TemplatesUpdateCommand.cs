// <copyright file="TemplatesUpdateCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Templates.Update
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.Linq;
    using Endjin.Adr.Cli.Configuration;
    using Endjin.Adr.Cli.Configuration.Contracts;

    public class TemplatesUpdateCommand : ICommandFactory<TemplatesUpdateCommand>
    {
        private readonly ITemplatePackageManager templateManager;
        private readonly ITemplateSettingsManager templateSettingsManager;

        public TemplatesUpdateCommand(ITemplatePackageManager templateManager, ITemplateSettingsManager templateSettingsManager)
        {
            this.templateManager = templateManager;
            this.templateSettingsManager = templateSettingsManager;
        }

        public Command Create()
        {
            return new Command("update", "Updates to the latest version of the ADR Templates Package.")
            {
                Handler = CommandHandler.Create(async () =>
                {
                    var currentSettings = this.templateSettingsManager.LoadSettings(nameof(TemplateSettings));

                    Console.WriteLine($"Current ADR Templates version {currentSettings.MetaData.Version}");

                    var templateMetaData = await this.templateManager.InstallLatestAsync(currentSettings.DefaultTemplatePackage).ConfigureAwait(false);
                    var defaultTemplate = templateMetaData.Details.Find(x => x.IsDefault) ?? templateMetaData.Details.First();

                    currentSettings.MetaData = templateMetaData;
                    currentSettings.DefaultTemplate = defaultTemplate.FullPath;

                    Console.WriteLine($"Downloaded ADR Templates version {templateMetaData.Version}");

                    this.templateSettingsManager.SaveSettings(currentSettings, nameof(TemplateSettings));

                    Console.WriteLine($"Updated ADR Templates to version {templateMetaData.Version}");
                }),
            };
        }
    }
}
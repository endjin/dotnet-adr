// <copyright file="TemplatesUpdateCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using Endjin.Adr.Cli.Configuration;
    using Endjin.Adr.Cli.Contracts;

    public class TemplatesUpdateCommand
    {
        private readonly ITemplatePackageManager templateManager;
        private readonly ITemplateSettingsMananger templateSettingsMananger;

        public TemplatesUpdateCommand(ITemplatePackageManager templateManager, ITemplateSettingsMananger templateSettingsMananger)
        {
            this.templateManager = templateManager;
            this.templateSettingsMananger = templateSettingsMananger;
        }

        public Command Create()
        {
            return new Command("update", "Updates to the latest version of the ADR Templates Package.")
            {
                Handler = CommandHandler.Create(async () =>
                {
                    var currentSettings = this.templateSettingsMananger.LoadSettings(nameof(TemplateSettings));

                    Console.WriteLine($"Current ADR Templates version {currentSettings.MetaData.Version}");

                    var templateMetaData = await this.templateManager.InstallLatestAsync(currentSettings.DefaultTemplatePackage).ConfigureAwait(false);
                    var templateSettings = new TemplateSettings
                    {
                        MetaData = templateMetaData,
                        DefaultTemplate = templateMetaData.Details.Find(x => x.IsDefault).FullPath,
                    };

                    Console.WriteLine($"Downloaded ADR Templates version {templateMetaData.Version}");

                    this.templateSettingsMananger.SaveSettings(templateSettings, nameof(TemplateSettings));

                    Console.WriteLine($"Updated ADR Templates to version {templateMetaData.Version}");
                }),
            };
        }
    }
}
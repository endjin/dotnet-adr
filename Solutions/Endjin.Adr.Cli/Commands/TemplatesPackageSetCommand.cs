// <copyright file="TemplatesPackageSetCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using Endjin.Adr.Cli.Configuration;
    using Endjin.Adr.Cli.Contracts;

    public class TemplatesPackageSetCommand
    {
        private readonly ITemplateSettingsMananger templateSettingsMananger;

        public TemplatesPackageSetCommand(ITemplateSettingsMananger templateSettingsMananger)
        {
            this.templateSettingsMananger = templateSettingsMananger;
        }

        public Command Create()
        {
            var cmd = new Command("set", "Sets the default NuGet ADR Template package.")
            {
                Handler = CommandHandler.Create((string packageId) =>
                {
                    if (!string.IsNullOrEmpty(packageId))
                    {
                        var templateSettings = this.templateSettingsMananger.LoadSettings(nameof(TemplateSettings));
                        templateSettings.DefaultTemplatePackage = packageId;

                        this.templateSettingsMananger.SaveSettings(templateSettings, nameof(TemplateSettings));

                        Console.WriteLine($"Setting \"{templateSettings.DefaultTemplatePackage}\" as the default NuGet ADR Template package.");
                    }
                }),
            };

            cmd.AddArgument(new Argument<string>("packageId"));

            return cmd;
        }
    }
}
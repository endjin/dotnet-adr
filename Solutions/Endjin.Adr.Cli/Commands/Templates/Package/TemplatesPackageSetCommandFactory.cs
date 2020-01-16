// <copyright file="TemplatesPackageSetCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Templates.Package
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using Endjin.Adr.Cli.Configuration;
    using Endjin.Adr.Cli.Configuration.Contracts;

    public class TemplatesPackageSetCommandFactory : ICommandFactory<TemplatesPackageSetCommandFactory>
    {
        private readonly ITemplateSettingsManager templateSettingsManager;

        public TemplatesPackageSetCommandFactory(ITemplateSettingsManager templateSettingsManager)
        {
            this.templateSettingsManager = templateSettingsManager;
        }

        public Command Create()
        {
            var cmd = new Command("set", "Sets the default NuGet ADR Template package.")
            {
                Handler = CommandHandler.Create((string packageId) =>
                {
                    if (string.IsNullOrEmpty(packageId))
                    {
                        return;
                    }

                    var templateSettings = this.templateSettingsManager.LoadSettings(nameof(TemplateSettings));
                    templateSettings.DefaultTemplatePackage = packageId;

                    this.templateSettingsManager.SaveSettings(templateSettings, nameof(TemplateSettings));

                    Console.WriteLine($"Setting \"{templateSettings.DefaultTemplatePackage}\" as the default NuGet ADR Template package.");
                }),
            };

            cmd.AddArgument(new Argument<string>("packageId"));

            return cmd;
        }
    }
}
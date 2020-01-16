// <copyright file="TemplatesPackageShowCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Templates.Package
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using Endjin.Adr.Cli.Configuration;
    using Endjin.Adr.Cli.Configuration.Contracts;

    public class TemplatesPackageShowCommandFactory : ICommandFactory<TemplatesPackageShowCommandFactory>
    {
        private readonly ITemplateSettingsManager templateSettingsManager;

        public TemplatesPackageShowCommandFactory(ITemplateSettingsManager templateSettingsManager)
        {
            this.templateSettingsManager = templateSettingsManager;
        }

        public Command Create()
        {
            return new Command("show", "Shows the default NuGet ADR Template package.")
            {
                Handler = CommandHandler.Create(() =>
                {
                    var templateSettings = this.templateSettingsManager.LoadSettings(nameof(TemplateSettings));

                    Console.WriteLine($"NuGet ADR Template Package: {templateSettings.DefaultTemplatePackage}");
                }),
            };
        }
    }
}
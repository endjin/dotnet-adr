// <copyright file="TemplatesPackageShowCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Templates.Package
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using Endjin.Adr.Cli.Configuration;
    using Endjin.Adr.Cli.Configuration.Contracts;

    public class TemplatesPackageShowCommand : ICommandFactory<TemplatesPackageShowCommand>
    {
        private readonly ITemplateSettingsManager templateSettingsManager;

        public TemplatesPackageShowCommand(ITemplateSettingsManager templateSettingsManager)
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
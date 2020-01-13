// <copyright file="TemplatesPackageShowCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using Endjin.Adr.Cli.Configuration;
    using Endjin.Adr.Cli.Contracts;

    public class TemplatesPackageShowCommand
    {
        private readonly ITemplateSettingsMananger templateSettingsMananger;

        public TemplatesPackageShowCommand(ITemplateSettingsMananger templateSettingsMananger)
        {
            this.templateSettingsMananger = templateSettingsMananger;
        }

        public Command Create()
        {
            return new Command("show", "Shows the default NuGet ADR Template package.")
            {
                Handler = CommandHandler.Create(() =>
                {
                    var templateSettings = this.templateSettingsMananger.LoadSettings(nameof(TemplateSettings));

                    Console.WriteLine($"NuGet ADR Template Package: {templateSettings.DefaultTemplatePackage}");
                }),
            };
        }
    }
}
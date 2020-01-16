// <copyright file="TemplatesDefaultShowCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Templates.Default
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.Globalization;
    using Endjin.Adr.Cli.Configuration;
    using Endjin.Adr.Cli.Configuration.Contracts;

    public class TemplatesDefaultShowCommandFactory : ICommandFactory<TemplatesDefaultShowCommandFactory>
    {
        private readonly ITemplateSettingsManager templateSettingsManager;

        public TemplatesDefaultShowCommandFactory(ITemplateSettingsManager templateSettingsManager)
        {
            this.templateSettingsManager = templateSettingsManager;
        }

        public Command Create()
        {
            return new Command("show", "Shows the default ADR Template.")
            {
                Handler = CommandHandler.Create(() =>
                {
                    var templateSettings = this.templateSettingsManager.LoadSettings(nameof(TemplateSettings));
                    var template = templateSettings.MetaData.Details.Find(x => x.FullPath == templateSettings.DefaultTemplate);

                    Console.WriteLine($"Title: {template.Title}");
                    Console.WriteLine($"Id: {template.Id}");
                    Console.WriteLine($"Description: {template.Description}");
                    Console.WriteLine($"Authors: {template.Authors}");
                    Console.WriteLine($"Effort: {template.Effort}");
                    Console.WriteLine($"More Info: {template.MoreInfo}");
                    Console.WriteLine($"Last Modified: {template.LastModified.ToString(CultureInfo.InvariantCulture)}");
                    Console.WriteLine($"Version: {template.Version}");
                }),
            };
        }
    }
}
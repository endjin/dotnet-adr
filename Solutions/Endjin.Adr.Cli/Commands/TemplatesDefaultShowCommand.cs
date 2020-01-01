// <copyright file="TemplatesDefaultShowCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using Endjin.Adr.Cli.Configuration;
    using Endjin.Adr.Cli.Contracts;

    public class TemplatesDefaultShowCommand
    {
        private readonly ITemplateSettingsMananger templateSettingsMananger;

        public TemplatesDefaultShowCommand(ITemplateSettingsMananger templateSettingsMananger)
        {
            this.templateSettingsMananger = templateSettingsMananger;
        }

        public Command Create()
        {
            return new Command("show", "Shows the default ADR Template.")
            {
                Handler = CommandHandler.Create(() =>
                {
                    var templateSettings = this.templateSettingsMananger.LoadSettings(nameof(TemplateSettings));
                    var template = templateSettings.MetaData.Details.Find(x => x.FullPath == templateSettings.DefaultTemplate);

                    Console.WriteLine($"Title: {template.Title}");
                    Console.WriteLine($"Id: {template.Id}");
                    Console.WriteLine($"Description: {template.Description}");
                    Console.WriteLine($"Authors: {template.Authors}");
                    Console.WriteLine($"Effort: {template.Effort}");
                    Console.WriteLine($"More Info: {template.MoreInfo}");
                    Console.WriteLine($"Last Modified: {template.LastModified.ToString()}");
                    Console.WriteLine($"Version: {template.Version}");
                }),
            };
        }
    }
}
// <copyright file="TemplatesCommandHandler.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using Endjin.Adr.Cli.Configuration;
    using Endjin.Adr.Cli.Contracts;

    public class TemplatesCommandHandler
    {
        private readonly ITemplateSettingsMananger templateSettingsMananger;

        public TemplatesCommandHandler(ITemplateSettingsMananger templateSettingsMananger)
        {
            this.templateSettingsMananger = templateSettingsMananger;
        }

        public Command Create()
        {
            var cmd = new Command("templates", "Perform operations on ADR Templates.");
            cmd.AddCommand(new Command("list", "Lists all installed ADR Templates.")
            {
                Handler = CommandHandler.Create(() =>
                {
                    var templateSettings = this.templateSettingsMananger.LoadSettings(nameof(TemplateSettings));

                    Console.WriteLine("=============================================================");

                    foreach (var template in templateSettings.MetaData.Details)
                    {
                        Console.WriteLine(string.Empty);
                        Console.WriteLine($" Title: {template.Title}");
                        Console.WriteLine($" Id: {template.Id}");
                        Console.WriteLine($" Description: {template.Description}");
                        Console.WriteLine($" Authors: {template.Authors}");
                        Console.WriteLine($" Effort: {template.Effort}");
                        Console.WriteLine($" More Info: {template.MoreInfo}");
                        Console.WriteLine($" Last Modified: {template.LastModified.ToString()}");
                        Console.WriteLine($" Version: {template.Version}");
                        Console.WriteLine(string.Empty);
                        Console.WriteLine("=============================================================");
                    }
                }),
            });
            cmd.AddCommand(new Command("install", "Install the specificed version of the ADR Templates Package."));
            cmd.AddCommand(new Command("update", "Updates to the latest version of the ADR Templates Package."));
            cmd.AddCommand(new Command("default", "Sets the default ADR Template to use."));

            return cmd;
        }
    }
}
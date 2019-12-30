// <copyright file="TemplatesCommandHandler.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.Linq;
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

            var defaultCmd = new Command("default", "Sets the default ADR Template to use. Use the template Id");

            var showDefaultSubCmd = new Command("show", "Shows the default ADR Template.")
            {
                Handler = CommandHandler.Create(() =>
                {
                    var templateSettings = this.templateSettingsMananger.LoadSettings(nameof(TemplateSettings));
                    var template = templateSettings.MetaData.Details.FirstOrDefault(x => x.FullPath == templateSettings.DefaultTemplate);

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

            defaultCmd.AddCommand(showDefaultSubCmd);

            var setDefaultSubCmd = new Command("set", "Sets the default ADR Template.")
            {
                Handler = CommandHandler.Create((string templateId) =>
                {
                    if (!string.IsNullOrEmpty(templateId))
                    {
                        var templateSettings = this.templateSettingsMananger.LoadSettings(nameof(TemplateSettings));
                        var template = templateSettings.MetaData.Details.FirstOrDefault(x => x.Id == templateId);
                        templateSettings.DefaultTemplate = template.FullPath;
                        this.templateSettingsMananger.SaveSettings(templateSettings, nameof(TemplateSettings));

                        Console.WriteLine($"Setting \"{template.Title}\" as the default ADR template.");
                    }
                }),
            };

            setDefaultSubCmd.AddArgument(new Argument<string>("templateId"));
            defaultCmd.AddCommand(setDefaultSubCmd);

            cmd.AddCommand(defaultCmd);

            var listSubCmd = new Command("list", "Lists all installed ADR Templates.")
            {
                Handler = CommandHandler.Create((bool idsOnly) =>
                {
                    var templateSettings = this.templateSettingsMananger.LoadSettings(nameof(TemplateSettings));

                    if (idsOnly)
                    {
                        foreach (var template in templateSettings.MetaData.Details)
                        {
                            Console.WriteLine(template.Id);
                        }
                    }
                    else
                    {
                        Console.WriteLine("-------");

                        foreach (var template in templateSettings.MetaData.Details)
                        {
                            Console.WriteLine(string.Empty);
                            Console.WriteLine($"Title: {template.Title}");
                            Console.WriteLine($"Id: {template.Id}");
                            Console.WriteLine($"Description: {template.Description}");
                            Console.WriteLine($"Authors: {template.Authors}");
                            Console.WriteLine($"Effort: {template.Effort}");
                            Console.WriteLine($"More Info: {template.MoreInfo}");
                            Console.WriteLine($"Last Modified: {template.LastModified.ToString()}");
                            Console.WriteLine($"Version: {template.Version}");
                            Console.WriteLine(string.Empty);
                            Console.WriteLine("-------");
                        }
                    }
                }),
            };

            listSubCmd.AddOption(new Option("ids-only"));

            cmd.AddCommand(listSubCmd);

            cmd.AddCommand(new Command("install", "Install the specificed version of the ADR Templates Package."));
            cmd.AddCommand(new Command("update", "Updates to the latest version of the ADR Templates Package."));

            return cmd;
        }
    }
}
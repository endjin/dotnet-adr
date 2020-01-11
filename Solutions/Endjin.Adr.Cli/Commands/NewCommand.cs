// <copyright file="NewCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands
{
    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Endjin.Adr.Cli.Configuration;
    using Endjin.Adr.Cli.Contracts;

    public class NewCommand
    {
        private readonly ITemplateSettingsMananger templateSettingsMananger;

        public NewCommand(ITemplateSettingsMananger templateSettingsMananger)
        {
            this.templateSettingsMananger = templateSettingsMananger;
        }

        public Command Create()
        {
            var cmd = new Command("new", "Creates a new ADR.")
            {
                Handler = CommandHandler.Create((int? id, string title) =>
                {
                    var adrs = new List<Adr>();
                    Regex fileNameRegExp = new Regex(@"(\d{4}.*\.md)");

                    foreach (var file in Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.md").Where(path => fileNameRegExp.IsMatch(path)))
                    {
                        var fileInfo = new FileInfo(file);
                        var existingAdr = new Adr
                        {
                            Path = file,
                            RecordNumber = int.Parse(fileInfo.Name.Substring(0, 4)),
                        };

                        adrs.Add(existingAdr);
                    }

                    var nextRecordNumber = adrs.OrderBy(x => x.RecordNumber).Last().RecordNumber + 1;

                    var adr = new Adr()
                    {
                        RecordNumber = nextRecordNumber,
                        Title = title,
                    };

                    var templateSettings = this.templateSettingsMananger.LoadSettings(nameof(TemplateSettings));
                    var template = templateSettings.MetaData.Details.Find(x => x.FullPath == templateSettings.DefaultTemplate);
                    var templateContents = File.ReadAllText(template.FullPath);

                    Regex yamlHeaderRegExp = new Regex(@"((?:^-{3})(?:.*\n)*(?:^-{3})\n# Title)", RegexOptions.Multiline);

                    adr.Content = yamlHeaderRegExp.Replace(templateContents, $"# {adr.Title}");

                    if (id.HasValue)
                    {
                        var superscede = adrs.Find(x => x.RecordNumber == id.Value);
                    }

                    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), adr.SafeFileName()), adr.Content);

                    Console.WriteLine($"Supercede ADR Record {id}");
                    Console.WriteLine($"Create ADR Record {title}");
                }),
            };

            cmd.AddOption(new Option("--id", "Id of ADR to supersede.") { Argument = new Argument<int>() });
            cmd.AddArgument(new Argument<string>("title") { Description = "Title of the ADR" });

            return cmd;
        }
    }
}
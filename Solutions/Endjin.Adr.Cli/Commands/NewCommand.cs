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
            var cmd = new Command("new", "Creates a new Architecural Decision Record, from the default ADR Template.")
            {
                Handler = CommandHandler.Create((int? id, string title) =>
                {
                    List<Adr> adrs = this.GetAllAdrFilesFromCurrentDirectory();

                    var adr = new Adr
                    {
                        Content = this.CreateNewDefaultTemplate(title),
                        RecordNumber = adrs.OrderBy(x => x.RecordNumber).Last().RecordNumber + 1,
                        Title = title,
                    };

                    if (id.HasValue)
                    {
                        var superscede = adrs.Find(x => x.RecordNumber == id.Value);

                        Regex superscededRegEx = new Regex(@"(?<=## Status.*\n)((?:.|\n)+?)(?=\n##)", RegexOptions.Multiline);

                        var updatedContent = superscededRegEx.Replace(superscede.Content, $"\nSupersceded by ADR {adr.RecordNumber.ToString("D4")} - {adr.Title}\n");

                        File.WriteAllText(superscede.Path, updatedContent);
                    }

                    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), adr.SafeFileName()), adr.Content);

                    Console.WriteLine($"Create ADR Record {title}");
                    Console.WriteLine($"Supercede ADR Record {id}");
                }),
            };

            cmd.AddOption(new Option("--id", "Id of ADR to supersede.") { Argument = new Argument<int>() });
            cmd.AddArgument(new Argument<string>("title") { Description = "Title of the ADR" });

            return cmd;
        }

        private string CreateNewDefaultTemplate(string title)
        {
            var templateSettings = this.templateSettingsMananger.LoadSettings(nameof(TemplateSettings));
            var template = templateSettings.MetaData.Details.Find(x => x.FullPath == templateSettings.DefaultTemplate);
            var templateContents = File.ReadAllText(template.FullPath);

            Regex yamlHeaderRegExp = new Regex(@"((?:^-{3})(?:.*\n)*(?:^-{3})\n# Title)", RegexOptions.Multiline);

            return yamlHeaderRegExp.Replace(templateContents, $"# {title}");
        }

        private List<Adr> GetAllAdrFilesFromCurrentDirectory()
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
                    Content = File.ReadAllText(file),
                };

                adrs.Add(existingAdr);
            }

            return adrs;
        }
    }
}
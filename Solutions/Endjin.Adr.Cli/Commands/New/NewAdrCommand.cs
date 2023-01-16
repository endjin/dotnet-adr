// <copyright file="NewAdrCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Endjin.Adr.Cli.Abstractions;
using Endjin.Adr.Cli.Configuration;
using Endjin.Adr.Cli.Configuration.Contracts;
using Endjin.Adr.Cli.Templates;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Endjin.Adr.Cli.Commands.New;

public class NewAdrCommand : AsyncCommand<NewAdrCommand.Settings>
{
    private readonly ITemplateSettingsManager templateSettingsManager;
    private readonly IAppEnvironmentManager appEnvironmentManager;

    public NewAdrCommand(ITemplateSettingsManager templateSettingsManager, IAppEnvironmentManager appEnvironmentManager)
    {
        this.templateSettingsManager = templateSettingsManager;
        this.appEnvironmentManager = appEnvironmentManager;
    }

    public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        try
        {
            List<Adr> adrs = GetAllAdrFilesFromCurrentDirectory();

            var adr = new Adr
            {
                Content = CreateNewDefaultTemplate(settings.Title, this.templateSettingsManager),
                RecordNumber = adrs.Count == 0 ? 1 : adrs.OrderBy(x => x.RecordNumber).Last().RecordNumber + 1,
                Title = settings.Title,
            };

            if (settings.Id.HasValue)
            {
                Adr supersede = adrs.Find(x => x.RecordNumber == settings.Id);

                Regex supersedeRegEx = new Regex(@"(?<=## Status.*\n)((?:.|\n)+?)(?=\n##)", RegexOptions.Multiline);

                string updatedContent = supersedeRegEx.Replace(supersede.Content, $"\nSuperseded by ADR {adr.RecordNumber:D4} - {adr.Title}\n");

                await File.WriteAllTextAsync(supersede.Path, updatedContent).ConfigureAwait(false);

                AnsiConsole.MarkupLine($"Supersede ADR Record: {settings.Id}");
            }

            await File.WriteAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), adr.SafeFileName()), adr.Content).ConfigureAwait(false);

            AnsiConsole.MarkupLine($"Create ADR Record: \"{settings.Title}\"");
        }
        catch (InvalidOperationException)
        {
            await this.appEnvironmentManager.SetFirstRunDesiredStateAsync().ConfigureAwait(false);
            await this.ExecuteAsync(context, settings).ConfigureAwait(false);
        }

        return ReturnCodes.Ok;
    }

    private static string CreateNewDefaultTemplate(string title, ITemplateSettingsManager templateSettingsManager)
    {
        TemplateSettings templateSettings = templateSettingsManager.LoadSettings(nameof(TemplateSettings));

        if (templateSettings is null)
        {
            throw new InvalidOperationException("Couldn't load the template settings. Environment may not be initialised");
        }

        TemplatePackageDetail template = templateSettings.MetaData.Details.Find(x => x.FullPath == templateSettings.DefaultTemplate);
        string templateContents = File.ReadAllText(template.FullPath);

        Regex yamlHeaderRegExp = new(@"((?:^-{3})(?:.*\n)*(?:^-{3})\n# Title)", RegexOptions.Multiline);

        return yamlHeaderRegExp.Replace(templateContents, $"# {title}");
    }

    private static List<Adr> GetAllAdrFilesFromCurrentDirectory()
    {
        List<Adr> adrs = new();
        Regex fileNameRegExp = new Regex(@"(\d{4}.*\.md)");

        foreach (string file in Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.md").Where(path => fileNameRegExp.IsMatch(path)))
        {
            FileInfo fileInfo = new FileInfo(file);
            Adr existingAdr = new Adr
            {
                Path = file,
                RecordNumber = int.Parse(fileInfo.Name.Substring(0, 4)),
                Content = File.ReadAllText(file),
            };

            adrs.Add(existingAdr);
        }

        return adrs;
    }

    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<TITLE>")]
        [Description("Title of the ADR")]
        public string Title { get; set; }

        [CommandOption("-i|--id <RECORDNUMBER>")]
        [Description("Id of ADR to supersede.")]
        public int? Id { get; set; }
    }
}
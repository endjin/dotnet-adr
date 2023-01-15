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
            var adrs = GetAllAdrFilesFromCurrentDirectory();

            var adr = new Adr
            {
                Content = CreateNewDefaultTemplate(settings.Title, this.templateSettingsManager),
                RecordNumber = adrs.Count == 0 ? 1 : adrs.OrderBy(x => x.RecordNumber).Last().RecordNumber + 1,
                Title = settings.Title,
            };

            if (settings.Id.HasValue)
            {
                var supersede = adrs.Find(x => x.RecordNumber == settings.Id);

                var supersedeRegEx = new Regex(@"(?<=## Status.*\n)((?:.|\n)+?)(?=\n##)", RegexOptions.Multiline);

                var updatedContent = supersedeRegEx.Replace(supersede.Content, $"\nSuperseded by ADR {adr.RecordNumber:D4} - {adr.Title}\n");

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
        var templateSettings = templateSettingsManager.LoadSettings(nameof(TemplateSettings));

        if (templateSettings is null)
        {
            throw new InvalidOperationException("Couldn't load the template settings. Environment may not be initialised");
        }

        var template = templateSettings.MetaData.Details.Find(x => x.FullPath == templateSettings.DefaultTemplate);
        var templateContents = File.ReadAllText(template.FullPath);

        var yamlHeaderRegExp = new Regex(@"((?:^-{3})(?:.*\n)*(?:^-{3})\n# Title)", RegexOptions.Multiline);

        return yamlHeaderRegExp.Replace(templateContents, $"# {title}");
    }

    private static List<Adr> GetAllAdrFilesFromCurrentDirectory()
    {
        var adrs = new List<Adr>();
        var fileNameRegExp = new Regex(@"(\d{4}.*\.md)");

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

    [Description("Add a NuGet package reference to the project")]
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<TITLE>")]
        [Description("The title of the ADR to add")]
        public string Title { get; set; }

        [CommandOption("-r|--record-number <RECORDNUMBER>")]
        [Description("The record number of the ADR to add")]
        public int? Id { get; set; }
    }
}
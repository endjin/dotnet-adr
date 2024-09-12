// <copyright file="NewAdrCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Endjin.Adr.Cli.Abstractions;
using Endjin.Adr.Cli.Configuration;
using Endjin.Adr.Cli.Configuration.Contracts;
using Endjin.Adr.Cli.Templates;

using Spectre.Console;
using Spectre.Console.Cli;

namespace Endjin.Adr.Cli.Commands.New;

public partial class NewAdrCommand : AsyncCommand<NewAdrCommand.Settings>
{
    private readonly ITemplateSettingsManager templateSettingsManager;
    private readonly IAppEnvironmentManager appEnvironmentManager;
    private readonly IConfigurationLocator configurationLocator;

    public NewAdrCommand(ITemplateSettingsManager templateSettingsManager, IAppEnvironmentManager appEnvironmentManager, IConfigurationLocator configurationLocator)
    {
        this.templateSettingsManager = templateSettingsManager;
        this.appEnvironmentManager = appEnvironmentManager;
        this.configurationLocator = configurationLocator;
    }

    public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        AnsiConsole.Write(new FigletText("dotnet-adr").Color(Color.Green));

        try
        {
            string targetPath = string.Empty;
            string templatePath = null;

            // If the user hasn't specified the path to create the ADR
            if (!string.IsNullOrEmpty(settings.Path))
            {
                targetPath = settings.Path;
            }
            else
            {
                // We'll attempt to see if there's a configuration file in the root of the "project".
                // We'll make the assumption that the root of the "project" is defined by the presence of
                // a ".git" directory, otherwise we'll just use the current location the ADR tool was launched from.
                string rootConfiguration = this.configurationLocator.LocatedRootConfiguration();

                if (!string.IsNullOrEmpty(rootConfiguration))
                {
                    string configText = await File.ReadAllTextAsync(rootConfiguration).ConfigureAwait(false);

                    JsonSerializerOptions options = new()
                    {
                        PropertyNameCaseInsensitive = true,
                    };

                    AdrConfig config = JsonSerializer.Deserialize<AdrConfig>(configText, options);
                    FileInfo rootConfigurationFileInfo = new(rootConfiguration);

                    // The configuration path is relative to config file.
                    targetPath = Path.GetFullPath(Path.Combine(rootConfigurationFileInfo.Directory.FullName, config.Path));

                    if (!rootConfigurationFileInfo.Directory.Exists)
                    {
                        rootConfigurationFileInfo.Directory.Create();
                    }

                    templatePath = config.TemplatePath;
                }

                if (string.IsNullOrEmpty(targetPath))
                {
                    targetPath = Environment.CurrentDirectory;
                }
            }

            List<Adr> documents = await GetAllAdrFilesFromCurrentDirectoryAsync(targetPath).ConfigureAwait(false);

            Adr adr = new()
            {
                Content = CreateNewDefaultTemplate(settings.Title, this.templateSettingsManager, templatePath),
                RecordNumber = documents.Count == 0 ? 1 : documents.OrderBy(x => x.RecordNumber).Last().RecordNumber + 1,
                Title = settings.Title,
            };

            if (settings.Id.HasValue)
            {
                // if an id has been specified, check to see if we're superseding an existing ADR, and mark it as updated, pointing to the new ADR.
                Adr supersede = documents.Find(x => x.RecordNumber == settings.Id);

                if (supersede is not null)
                {
                    Regex supersedeRegEx = SupersedeRegex();

                    string updatedContent = supersedeRegEx.Replace(supersede.Content, $"\nSuperseded by ADR {adr.RecordNumber:D4} - {adr.Title}\n");

                    await File.WriteAllTextAsync(supersede.Path, updatedContent).ConfigureAwait(false);

                    AnsiConsole.MarkupLine($"Superseded ADR Record: [aqua]{settings.Id}[/]");
                }
            }

            await File.WriteAllTextAsync(Path.Combine(targetPath, adr.SafeFileName()), adr.Content).ConfigureAwait(false);

            AnsiConsole.MarkupLine($"""Created ADR Record: [aqua]"{settings.Title}"[/] in [yellow]{targetPath}[/]""");
        }
        catch (InvalidOperationException)
        {
            await this.appEnvironmentManager.SetFirstRunDesiredStateAsync().ConfigureAwait(false);
            await this.ExecuteAsync(context, settings).ConfigureAwait(false);
        }

        return ReturnCodes.Ok;
    }

    private static string CreateNewDefaultTemplate(string title, ITemplateSettingsManager templateSettingsManager, string templatePath)
    {
        TemplateSettings templateSettings = templateSettingsManager.LoadSettings(nameof(TemplateSettings));

        if (templateSettings is null)
        {
            throw new InvalidOperationException("Couldn't load the template settings. Environment may not be initialised");
        }

        TemplatePackageDetail defaultTemplate = templateSettings.MetaData.Details.Find(x => x.FullPath == templateSettings.DefaultTemplate);

        string templateContents = File.ReadAllText(templatePath ?? defaultTemplate.FullPath);

        Regex yamlHeaderRegExp = YamlHeaderRegex();

        return yamlHeaderRegExp
            .Replace(templateContents, $"# {title}")
            .Replace("{DATE}", DateTime.Now.ToShortDateString())
            .Replace("{TITLE}", title);
    }

    private static async Task<List<Adr>> GetAllAdrFilesFromCurrentDirectoryAsync(string targetPath)
    {
        if (!Directory.Exists(targetPath))
        {
            Directory.CreateDirectory(targetPath);
        }

        List<Adr> documents = new();
        Regex fileNameRegExp = FileNameRegex();

        foreach (string file in Directory.EnumerateFiles(targetPath, "*.md").Where(path => fileNameRegExp.IsMatch(path)))
        {
            FileInfo fileInfo = new(file);
            Adr existingAdr = new()
            {
                Path = file,
                RecordNumber = int.Parse(fileInfo.Name[..4]),
                Content = await File.ReadAllTextAsync(file).ConfigureAwait(false),
            };

            documents.Add(existingAdr);
        }

        return documents;
    }

    [GeneratedRegex(@"(\d{4}.*\.md)")]
    private static partial Regex FileNameRegex();

    [GeneratedRegex(@"((?:^-{3})(?:.*\n)*(?:^-{3})\n# Title)", RegexOptions.Multiline)]
    private static partial Regex YamlHeaderRegex();

    [GeneratedRegex(@"(?<=## Status.*\n)((?:.|\n)+?)(?=\n##)", RegexOptions.Multiline)]
    private static partial Regex SupersedeRegex();

    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<TITLE>")]
        [Description("Title of the ADR")]
        public string Title { get; set; }

        [CommandOption("-i|--id <RECORDNUMBER>")]
        [Description("Id of ADR to supersede.")]
        public int? Id { get; set; }

        [CommandOption("-p|--path <PATH>")]
        [Description("Path to create the ADR in")]
        public string Path { get; set; }
    }
}
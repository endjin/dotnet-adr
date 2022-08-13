// <copyright file="NewAdrHandler.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.New
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text.RegularExpressions;
  using System.Threading.Tasks;

  using Endjin.Adr.Cli.Abstractions;
  using Endjin.Adr.Cli.Configuration;
  using Endjin.Adr.Cli.Configuration.Contracts;
  using Endjin.Adr.Cli.Infrastructure;

  using Spectre.Console;

  public static class NewAdrHandler
  {
    public static Task<int> ExecuteAsync(string title, int? id, ICompositeConsole console, ITemplateSettingsManager templateSettingsManager)
    {
      List<Adr> adrs = GetAllAdrFilesFromCurrentDirectory();

      var adr = new Adr
      {
        Content = CreateNewDefaultTemplate(title, templateSettingsManager),
        RecordNumber = adrs.Count == 0 ? 1 : adrs.OrderBy(x => x.RecordNumber).Last().RecordNumber + 1,
        Title = title,
      };

      if (id.HasValue)
      {
        var supersede = adrs.Find(x => x.RecordNumber == id);

        Regex supersedeRegEx = new Regex(@"(?<=## Status.*\n)((?:.|\n)+?)(?=\n##)", RegexOptions.Multiline);

        var updatedContent = supersedeRegEx.Replace(supersede.Content, $"\nSuperseded by ADR {adr.RecordNumber:D4} - {adr.Title}\n");

        File.WriteAllText(supersede.Path, updatedContent);

        console.WriteLine($"Supersede ADR Record: {id}");
      }

      File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), adr.SafeFileName()), adr.Content);

      console.WriteLine($"Create ADR Record: \"{title}\"");

      return Task.FromResult(ReturnCodes.Ok);
    }

    private static string CreateNewDefaultTemplate(string title, ITemplateSettingsManager templateSettingsManager)
    {
      var templateSettings = templateSettingsManager.LoadSettings(nameof(TemplateSettings));
      var template = templateSettings.MetaData.Details.Find(x => x.FullPath == templateSettings.DefaultTemplate);
      var templateContents = File.ReadAllText(template.FullPath);

      Regex yamlHeaderRegExp = new Regex(@"((?:^-{3})(?:.*\n)*(?:^-{3})\n# Title)", RegexOptions.Multiline);

      return yamlHeaderRegExp.Replace(templateContents, $"# {title}");
    }

    private static List<Adr> GetAllAdrFilesFromCurrentDirectory()
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
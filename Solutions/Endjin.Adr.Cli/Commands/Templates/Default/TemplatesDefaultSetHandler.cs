// <copyright file="TemplatesDefaultSetHandler.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Templates.Default
{
  using System;
  using System.CommandLine;
  using System.Globalization;
  using System.Threading.Tasks;

  using Endjin.Adr.Cli.Abstractions;
  using Endjin.Adr.Cli.Configuration;
  using Endjin.Adr.Cli.Configuration.Contracts;
  using Endjin.Adr.Cli.Infrastructure;

  public static class TemplatesDefaultSetHandler
  {
    public static Task<int> ExecuteAsync(string templateId, ICompositeConsole console, ITemplateSettingsManager templateSettingsManager)
    {
      if (!string.IsNullOrEmpty(templateId))
      {
        var templateSettings = templateSettingsManager.LoadSettings(nameof(TemplateSettings));
        var template = templateSettings.MetaData.Details.Find(x => x.Id == templateId);

        templateSettings.DefaultTemplate = template.FullPath;

        templateSettingsManager.SaveSettings(templateSettings, nameof(TemplateSettings));

        console.WriteLine($"Setting \"{template.Title}\" as the default ADR template.");

        return Task.FromResult(ReturnCodes.Ok);
      }

      return Task.FromResult(ReturnCodes.Error);
    }
  }
}
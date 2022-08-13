// <copyright file="TemplatesUpdateHandler.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Templates.Update
{
  using System;
  using System.Globalization;
  using System.Threading.Tasks;

  using Endjin.Adr.Cli.Abstractions;
  using Endjin.Adr.Cli.Configuration;
  using Endjin.Adr.Cli.Configuration.Contracts;
  using Endjin.Adr.Cli.Infrastructure;

  public static class TemplatesUpdateHandler
  {
    public static async Task<int> ExecuteAsync(bool idsOnly, ICompositeConsole console, ITemplateSettingsManager templateSettingsManager, ITemplatePackageManager templatePackageManager)
    {
      var currentSettings = templateSettingsManager.LoadSettings(nameof(TemplateSettings));

      Console.WriteLine($"Current ADR Templates version {currentSettings.MetaData.Version}");

      var templateMetaData = await templatePackageManager.InstallLatestAsync(currentSettings.DefaultTemplatePackage).ConfigureAwait(false);
      var defaultTemplate = templateMetaData.Details.Find(x => x.IsDefault) ?? templateMetaData.Details[0];

      currentSettings.MetaData = templateMetaData;
      currentSettings.DefaultTemplate = defaultTemplate.FullPath;

      Console.WriteLine($"Downloaded ADR Templates version {templateMetaData.Version}");

      templateSettingsManager.SaveSettings(currentSettings, nameof(TemplateSettings));

      Console.WriteLine($"Updated ADR Templates to version {templateMetaData.Version}");

      return ReturnCodes.Ok;
    }
  }
}
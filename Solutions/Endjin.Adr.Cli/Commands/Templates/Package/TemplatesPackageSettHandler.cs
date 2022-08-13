// <copyright file="TemplatesPackageSettHandler.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Templates.Package
{
  using System;
  using System.Threading.Tasks;

  using Endjin.Adr.Cli.Abstractions;
  using Endjin.Adr.Cli.Configuration;
  using Endjin.Adr.Cli.Configuration.Contracts;
  using Endjin.Adr.Cli.Infrastructure;

  public static class TemplatesPackageSettHandler
  {
    public static Task<int> ExecuteAsync(string packageId, ICompositeConsole console, ITemplateSettingsManager templateSettingsManager)
    {
      if (string.IsNullOrEmpty(packageId))
      {
        return Task.FromResult(ReturnCodes.Error);
      }

      var templateSettings = templateSettingsManager.LoadSettings(nameof(TemplateSettings));
      templateSettings.DefaultTemplatePackage = packageId;

      templateSettingsManager.SaveSettings(templateSettings, nameof(TemplateSettings));

      Console.WriteLine($"Setting \"{templateSettings.DefaultTemplatePackage}\" as the default NuGet ADR Template package.");

      return Task.FromResult(ReturnCodes.Ok);
    }
  }
}
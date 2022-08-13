// <copyright file="TemplatesPackageShowHandler.cs" company="Endjin Limited">
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

  public static class TemplatesPackageShowHandler
  {
    public static Task<int> ExecuteAsync(ICompositeConsole console, ITemplateSettingsManager templateSettingsManager)
    {
      var templateSettings = templateSettingsManager.LoadSettings(nameof(TemplateSettings));

      Console.WriteLine($"NuGet ADR Template Package: {templateSettings.DefaultTemplatePackage}");

      return Task.FromResult(ReturnCodes.Ok);
    }
  }
}
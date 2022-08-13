// <copyright file="TemplatesListHandler.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Templates.List
{
  using System;
  using System.Globalization;
  using System.Threading.Tasks;

  using Endjin.Adr.Cli.Abstractions;
  using Endjin.Adr.Cli.Configuration;
  using Endjin.Adr.Cli.Configuration.Contracts;
  using Endjin.Adr.Cli.Infrastructure;

  public static class TemplatesListHandler
  {
    public static Task<int> ExecuteAsync(bool idsOnly, ICompositeConsole console, ITemplateSettingsManager templateSettingsManager)
    {
      var templateSettings = templateSettingsManager.LoadSettings(nameof(TemplateSettings));

      if (idsOnly)
      {
        foreach (var template in templateSettings.MetaData.Details)
        {
          Console.WriteLine(template.Id);
        }
      }
      else
      {
        Console.WriteLine("-------");

        foreach (var template in templateSettings.MetaData.Details)
        {
          Console.WriteLine(string.Empty);
          Console.WriteLine($"Title: {template.Title}");
          Console.WriteLine($"Id: {template.Id}");
          Console.WriteLine($"Description: {template.Description}");
          Console.WriteLine($"Authors: {template.Authors}");
          Console.WriteLine($"Effort: {template.Effort}");
          Console.WriteLine($"More Info: {template.MoreInfo}");
          Console.WriteLine($"Last Modified: {template.LastModified.ToString(CultureInfo.InvariantCulture)}");
          Console.WriteLine($"Version: {template.Version}");
          Console.WriteLine(string.Empty);
          Console.WriteLine("-------");
        }
      }

      return Task.FromResult(ReturnCodes.Ok);
    }
  }
}
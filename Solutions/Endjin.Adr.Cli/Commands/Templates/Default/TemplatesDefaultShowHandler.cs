// <copyright file="TemplatesDefaultShowHandler.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Templates.Default
{
  using System.CommandLine;
  using System.Globalization;
  using System.Threading.Tasks;
  using Endjin.Adr.Cli.Abstractions;
  using Endjin.Adr.Cli.Configuration;
  using Endjin.Adr.Cli.Configuration.Contracts;
  using Endjin.Adr.Cli.Infrastructure;

  public static class TemplatesDefaultShowHandler
  {
    public static Task<int> ExecuteAsync(string path, ICompositeConsole console, ITemplateSettingsManager templateSettingsManager)
    {
      var templateSettings = templateSettingsManager.LoadSettings(nameof(TemplateSettings));
      var template = templateSettings.MetaData.Details.Find(x => x.FullPath == templateSettings.DefaultTemplate);

      console.WriteLine($"Title: {template.Title}");
      console.WriteLine($"Id: {template.Id}");
      console.WriteLine($"Description: {template.Description}");
      console.WriteLine($"Authors: {template.Authors}");
      console.WriteLine($"Effort: {template.Effort}");
      console.WriteLine($"More Info: {template.MoreInfo}");
      console.WriteLine($"Last Modified: {template.LastModified.ToString(CultureInfo.InvariantCulture)}");
      console.WriteLine($"Version: {template.Version}");

      return Task.FromResult(ReturnCodes.Ok);
    }
  }
}
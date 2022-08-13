// <copyright file="EnvironmentInitHandler.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Init
{
  using System.IO;
  using System.Threading.Tasks;
  using Endjin.Adr.Cli.Abstractions;
  using Endjin.Adr.Cli.Infrastructure;
  using Spectre.Console;

  public static class EnvironmentInitHandler
  {
    public static Task<int> ExecuteAsync(string path, ICompositeConsole console)
    {
      // await appEnvironment.InitializeAsync(console).ConfigureAwait(false);
      if (string.IsNullOrEmpty(path))
      {
        path = Path.Combine(Directory.GetCurrentDirectory(), "docs", "adr");
      }

      if (!Directory.Exists(path))
      {
        Directory.CreateDirectory(path);

        console.Write(new Text($"Created ADR Repository in '{path}'"));
      }
      else
      {
        console.Write(new Text($"'{path}' already exists."));
      }

      return Task.FromResult(ReturnCodes.Ok);
    }
  }
}
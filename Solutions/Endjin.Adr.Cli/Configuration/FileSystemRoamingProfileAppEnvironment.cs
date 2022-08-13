// <copyright file="FileSystemRoamingProfileAppEnvironment.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Configuration
{
  using System;
  using System.Collections.Generic;
  using System.CommandLine;
  using System.CommandLine.IO;
  using System.Diagnostics;
  using System.IO;
  using System.Linq;
  using System.Threading.Tasks;
  using Endjin.Adr.Cli.Configuration.Contracts;
  using NDepend.Path;

  public class FileSystemRoamingProfileAppEnvironment : IAppEnvironment
  {
    public const string AppName = "dotnet-adr";
    public const string AppOrgName = "endjin";
    public const string ConfigurationDirectorName = "configuration";
    public const string TemplatesDirectoryName = "templates";
    public const string NuGetFileName = "NuGet.Config";
    public const string PluginsDirectoryName = "plugins";
    public const string DefaultNuGetConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""NuGet official package source"" value=""https://api.nuget.org/v3/index.json"" />
    </packageSources>
</configuration>";

    private readonly IConsole console;

    public FileSystemRoamingProfileAppEnvironment(IConsole console)
    {
      this.console = console;
    }

    public IAbsoluteDirectoryPath AppPath
    {
      get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppOrgName, AppName).ToAbsoluteDirectoryPath(); }
    }

    public IAbsoluteDirectoryPath TemplatesPath
    {
      get { return Path.Combine(this.AppPath.ToString(), TemplatesDirectoryName).ToAbsoluteDirectoryPath(); }
    }

    public IAbsoluteDirectoryPath PluginPath
    {
      get { return Path.Combine(this.AppPath.ToString(), PluginsDirectoryName).ToAbsoluteDirectoryPath(); }
    }

    public IEnumerable<IAbsoluteDirectoryPath> PluginPaths
    {
      get
      {
        if (Debugger.IsAttached)
        {
          string directory = AppContext.BaseDirectory;

          while (!Directory.Exists(Path.Combine(directory, ".git")) && directory != null)
          {
            directory = Directory.GetParent(directory).FullName;
          }

          IEnumerable<string> dirs = Directory.EnumerateDirectories(directory, "*.*", SearchOption.AllDirectories)
            .Where(f => !Directory.EnumerateDirectories(f, "*.*", SearchOption.TopDirectoryOnly).Any() &&
                        f.EndsWith(@"bin\Debug\net6.0"));

          foreach (string dir in dirs)
          {
            yield return dir.ToAbsoluteDirectoryPath();
          }
        }
        else
        {
          foreach (IAbsoluteDirectoryPath path in this.PluginPath.ChildrenDirectoriesPath)
          {
            IEnumerable<IAbsoluteDirectoryPath> leafPaths = Directory
              .EnumerateDirectories(path.ToString(), "*.*", SearchOption.AllDirectories)
              .Where(f => !Directory.EnumerateDirectories(f, "*.*", SearchOption.TopDirectoryOnly).Any()).Select(x => x.ToAbsoluteDirectoryPath());

            foreach (IAbsoluteDirectoryPath leafPath in leafPaths)
            {
              yield return leafPath;
            }
          }
        }
      }
    }

    public IAbsoluteDirectoryPath ConfigurationPath
    {
      get { return Path.Combine(this.AppPath.ToString(), ConfigurationDirectorName).ToAbsoluteDirectoryPath(); }
    }

    public IAbsoluteFilePath NuGetConfigFilePath
    {
      get { return Path.Combine(this.ConfigurationPath.ToString(), NuGetFileName).ToAbsoluteFilePath(); }
    }

    public void Clean()
    {
      Directory.Delete(this.AppPath.ToString(), recursive: true);
    }

    public async Task InitializeAsync(IConsole console)
    {
      if (!Directory.Exists(this.AppPath.ToString()))
      {
        console.Out.WriteLine($"Creating {this.AppPath}");
        Directory.CreateDirectory(this.AppPath.ToString());
      }

      if (!Directory.Exists(this.ConfigurationPath.ToString()))
      {
        console.Out.WriteLine($"Creating {this.ConfigurationPath}");
        Directory.CreateDirectory(this.ConfigurationPath.ToString());
      }

      using (StreamWriter writer = File.CreateText(this.NuGetConfigFilePath.ToString()))
      {
        console.Out.WriteLine($"Creating {this.NuGetConfigFilePath}");
        await writer.WriteAsync(DefaultNuGetConfig).ConfigureAwait(false);
      }

      if (!Directory.Exists(this.PluginPath.ToString()))
      {
        console.Out.WriteLine($"Creating {this.PluginPath}");
        Directory.CreateDirectory(this.PluginPath.ToString());
      }

      if (!Directory.Exists(this.TemplatesPath.ToString()))
      {
        console.Out.WriteLine($"Creating {this.TemplatesPath}");
        Directory.CreateDirectory(this.TemplatesPath.ToString());
      }
    }

    public bool IsInitialized()
    {
      return Directory.Exists(this.AppPath.ToString()) &&
             Directory.Exists(this.TemplatesPath.ToString()) &&
             Directory.Exists(this.ConfigurationPath.ToString()) &&
             Directory.Exists(this.TemplatesPath.ToString()) &&
             Directory.Exists(this.PluginPath.ToString());
    }
  }
}
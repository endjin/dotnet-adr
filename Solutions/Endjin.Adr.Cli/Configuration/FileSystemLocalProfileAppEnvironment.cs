// <copyright file="FileSystemLocalProfileAppEnvironment.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Endjin.Adr.Cli.Configuration.Contracts;
using Endjin.Adr.Cli.Extensions;
using Spectre.Console;
using Spectre.IO;
using Environment = System.Environment;
using Path = System.IO.Path;

namespace Endjin.Adr.Cli.Configuration;

public class FileSystemLocalProfileAppEnvironment : IAppEnvironment
{
    public const string AppName = "adr";
    public const string AppOrgName = "endjin";
    public const string ConfigurationDirectorName = "configuration";
    public const string PluginsDirectoryName = "plugins";
    public const string TemplatesDirectoryName = "templates";
    public const string NuGetFileName = "NuGet.Config";

    public const string DefaultNuGetConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""NuGet official package source"" value=""https://api.nuget.org/v3/index.json"" />
    </packageSources>
</configuration>";

    public DirectoryPath AppPath
    {
        get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppOrgName, AppName); }
    }

    public DirectoryPath TemplatesPath
    {
        get { return Path.Combine(this.AppPath.ToString(), TemplatesDirectoryName); }
    }

    public DirectoryPath PluginPath
    {
        get { return Path.Combine(this.AppPath.ToString(), PluginsDirectoryName); }
    }

    public IEnumerable<DirectoryPath> PluginPaths
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

                IEnumerable<string> dirs = Directory
                        .EnumerateDirectories(directory, "*.*", SearchOption.AllDirectories)
                        .Where(f => !Directory.EnumerateDirectories(f, "*.*", SearchOption.TopDirectoryOnly).Any() && f.EndsWith(@"bin\Debug\net6.0"));

                foreach (string dir in dirs)
                {
                    yield return dir;
                }
            }
            else
            {
                foreach (DirectoryPath path in this.PluginPath.ChildrenDirectoriesPath())
                {
                    IEnumerable<DirectoryPath> leafPaths = Directory
                            .EnumerateDirectories(path.ToString(), "*.*", SearchOption.AllDirectories)
                            .Where(f => !Directory.EnumerateDirectories(f, "*.*", SearchOption.TopDirectoryOnly).Any()).Select(x => new DirectoryPath(x));

                    foreach (DirectoryPath leafPath in leafPaths)
                    {
                        yield return leafPath;
                    }
                }
            }
        }
    }

    public DirectoryPath ConfigurationPath
    {
        get { return Path.Combine(this.AppPath.ToString(), ConfigurationDirectorName); }
    }

    public FilePath NuGetConfigFilePath
    {
        get { return Path.Combine(this.ConfigurationPath.ToString(), NuGetFileName); }
    }

    public void Clean()
    {
        Directory.Delete(this.AppPath.ToString(), recursive: true);
    }

    public async Task InitializeAsync()
    {
        if (!Directory.Exists(this.AppPath.ToString()))
        {
            AnsiConsole.MarkupLine($"Creating {this.AppPath}");
            Directory.CreateDirectory(this.AppPath.ToString());
        }

        if (!Directory.Exists(this.ConfigurationPath.ToString()))
        {
            AnsiConsole.MarkupLine($"Creating {this.ConfigurationPath}");
            Directory.CreateDirectory(this.ConfigurationPath.ToString());
        }

        await using (StreamWriter writer = File.CreateText(this.NuGetConfigFilePath.ToString()))
        {
            AnsiConsole.MarkupLine($"Creating {this.NuGetConfigFilePath}");
            await writer.WriteAsync(DefaultNuGetConfig).ConfigureAwait(false);
        }

        if (!Directory.Exists(this.PluginPath.ToString()))
        {
            AnsiConsole.MarkupLine($"Creating {this.PluginPath}");
            Directory.CreateDirectory(this.PluginPath.ToString());
        }

        if (!Directory.Exists(this.TemplatesPath.ToString()))
        {
            AnsiConsole.MarkupLine($"Creating {this.TemplatesPath}");
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
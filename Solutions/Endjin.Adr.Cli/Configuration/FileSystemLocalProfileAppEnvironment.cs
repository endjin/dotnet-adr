// <copyright file="FileSystemLocalProfileAppEnvironment.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>
namespace Endjin.Adr.Cli.Configuration
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Endjin.Adr.Cli.Configuration.Contracts;

    public class FileSystemLocalProfileAppEnvironment : IAppEnvironment
    {
        public const string AppName = "dotnet-adr";
        public const string AppOrgName = "endjin";
        public const string ConfigurationDirectorName = "configuration";
        public const string TemplatesDirectoryName = "templates";
        public const string NuGetFileName = "NuGet.Config";

        public string AppPath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppOrgName, AppName);
            }
        }

        public string TemplatesPath
        {
            get
            {
                return Path.Combine(this.AppPath, TemplatesDirectoryName);
            }
        }

        public string ConfigurationPath
        {
            get { return Path.Combine(this.AppPath, ConfigurationDirectorName); }
        }

        public string NuGetConfigFilePath
        {
            get { return Path.Combine(this.ConfigurationPath, "NuGet.Config"); }
        }

        public void Clean()
        {
            Directory.Delete(this.AppPath, recursive: true);
        }

        public async Task InitializeAsync()
        {
            if (!Directory.Exists(this.AppPath))
            {
                Directory.CreateDirectory(this.AppPath);
            }

            if (!Directory.Exists(this.ConfigurationPath))
            {
                Directory.CreateDirectory(this.ConfigurationPath);
            }

            if (!Directory.Exists(this.TemplatesPath))
            {
                Directory.CreateDirectory(this.TemplatesPath);
            }

            using (var writer = File.CreateText(this.NuGetConfigFilePath))
            {
                await writer.WriteAsync(this.DefaultNuGetConfig()).ConfigureAwait(false);
            }
        }

        public bool IsInitialized()
        {
            // TODO: Better probing that a template actually exists.
            return Directory.Exists(this.TemplatesPath);
        }

        private string DefaultNuGetConfig()
        {
            return @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""NuGet official package source"" value=""https://api.nuget.org/v3/index.json"" />
    </packageSources>
</configuration>";
        }
    }
}
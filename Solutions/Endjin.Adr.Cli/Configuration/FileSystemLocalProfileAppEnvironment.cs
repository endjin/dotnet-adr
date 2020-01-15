// <copyright file="FileSystemLocalProfileAppEnvironment.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Configuration
{
    using System;
    using System.IO;
    using Endjin.Adr.Cli.Contracts;

    public class FileSystemLocalProfileAppEnvironment : IAppEnvironment
    {
        public const string AppName = "dotnet-adr";
        public const string AppOrgName = "endjin";
        public const string ConfigurationDirectorName = "configuration";
        public const string TemplatesDirectoryName = "templates";

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

        public void Initialize()
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
        }

        public void Clean()
        {
            Directory.Delete(this.AppPath, recursive: true);
        }

        public bool IsInitialized()
        {
            // TODO: Better probing that a template actually exists.
            return Directory.Exists(this.TemplatesPath);
        }
    }
}
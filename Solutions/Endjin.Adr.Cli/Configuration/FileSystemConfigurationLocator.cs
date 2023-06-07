// <copyright file="FileSystemConfigurationLocator.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.IO;

using Endjin.Adr.Cli.Configuration.Contracts;

namespace Endjin.Adr.Cli.Configuration;

public class FileSystemConfigurationLocator : IConfigurationLocator
{
    public string LocatedRootConfiguration()
    {
        string directory = Environment.CurrentDirectory;

        while (!Directory.Exists(Path.Combine(directory, ".git")))
        {
            directory = Directory.GetParent(directory).FullName;
        }

        string configurationPath = Path.Combine(directory, "adr.config.json");

        return File.Exists(configurationPath) ? configurationPath : string.Empty;
    }
}
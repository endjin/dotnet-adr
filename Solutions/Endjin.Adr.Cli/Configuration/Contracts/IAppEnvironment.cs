// <copyright file="IAppEnvironment.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using Spectre.IO;

namespace Endjin.Adr.Cli.Configuration.Contracts;

public interface IAppEnvironment : IAppEnvironmentConfiguration
{
    FilePath NuGetConfigFilePath { get; }

    DirectoryPath TemplatesPath { get; }

    DirectoryPath PluginPath { get; }

    IEnumerable<DirectoryPath> PluginPaths { get; }

    void Clean();

    Task InitializeAsync();

    bool IsInitialized();
}
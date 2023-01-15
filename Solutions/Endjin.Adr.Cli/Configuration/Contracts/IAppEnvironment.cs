// <copyright file="IAppEnvironment.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using NDepend.Path;

namespace Endjin.Adr.Cli.Configuration.Contracts;
public interface IAppEnvironment : IAppEnvironmentConfiguration
{
    IAbsoluteFilePath NuGetConfigFilePath { get; }

    IAbsoluteDirectoryPath TemplatesPath { get; }

    IAbsoluteDirectoryPath PluginPath { get; }

    IEnumerable<IAbsoluteDirectoryPath> PluginPaths { get; }

    void Clean();

    Task InitializeAsync();

    bool IsInitialized();
}
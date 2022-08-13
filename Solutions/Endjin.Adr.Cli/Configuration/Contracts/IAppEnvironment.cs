// <copyright file="IAppEnvironment.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Configuration.Contracts
{
    using System.Collections.Generic;
    using System.CommandLine;
    using System.Threading.Tasks;

    using NDepend.Path;

    public interface IAppEnvironment : IAppEnvironmentConfiguration
    {
      IAbsoluteFilePath NuGetConfigFilePath { get; }

      IAbsoluteDirectoryPath TemplatesPath { get; }

      IAbsoluteDirectoryPath PluginPath { get; }

      IEnumerable<IAbsoluteDirectoryPath> PluginPaths { get; }

      void Clean();

      Task InitializeAsync(IConsole console);

      bool IsInitialized();
    }
}
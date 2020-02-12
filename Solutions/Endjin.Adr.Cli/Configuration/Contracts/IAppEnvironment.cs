// <copyright file="IAppEnvironment.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Configuration.Contracts
{
    using System.Threading.Tasks;

    public interface IAppEnvironment
    {
        string AppPath { get; }

        string ConfigurationPath { get; }

        string NuGetConfigFilePath { get; }

        string TemplatesPath { get; }

        void Clean();

        Task InitializeAsync();

        bool IsInitialized();
    }
}
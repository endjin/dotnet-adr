// <copyright file="IAppEnvironment.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Configuration.Contracts
{
    public interface IAppEnvironment
    {
        string AppPath { get; }

        string ConfigurationPath { get; }

        string TemplatesPath { get; }

        void Clean();

        void Initialize();

        bool IsInitialized();
    }
}
// <copyright file="IAppEnvironmentConfiguration.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Configuration.Contracts
{
    using NDepend.Path;

    public interface IAppEnvironmentConfiguration
    {
        IAbsoluteDirectoryPath AppPath { get; }

        IAbsoluteDirectoryPath ConfigurationPath { get; }
    }
}
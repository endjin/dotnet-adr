// <copyright file="IAppEnvironmentConfiguration.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Spectre.IO;

namespace Endjin.Adr.Cli.Configuration.Contracts;

public interface IAppEnvironmentConfiguration
{
    DirectoryPath AppPath { get; }

    DirectoryPath ConfigurationPath { get; }
}
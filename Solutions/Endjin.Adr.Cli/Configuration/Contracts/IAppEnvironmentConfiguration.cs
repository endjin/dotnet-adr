// <copyright file="IAppEnvironmentConfiguration.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using NDepend.Path;

namespace Endjin.Adr.Cli.Configuration.Contracts;
public interface IAppEnvironmentConfiguration
{
    IAbsoluteDirectoryPath AppPath { get; }

    IAbsoluteDirectoryPath ConfigurationPath { get; }
}
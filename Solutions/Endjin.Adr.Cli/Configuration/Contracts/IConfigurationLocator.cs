// <copyright file="IConfigurationLocator.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Configuration.Contracts;

public interface IConfigurationLocator
{
    string LocatedRootConfiguration();
}
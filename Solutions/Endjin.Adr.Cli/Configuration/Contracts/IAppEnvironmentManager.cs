// <copyright file="IAppEnvironmentManager.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Endjin.Adr.Cli.Configuration.Contracts;

public interface IAppEnvironmentManager
{
    Task ResetDesiredStateAsync();

    Task SetDesiredStateAsync();

    Task SetFirstRunDesiredStateAsync();
}
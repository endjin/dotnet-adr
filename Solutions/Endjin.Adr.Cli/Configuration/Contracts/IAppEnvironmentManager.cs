// <copyright file="IAppEnvironmentManager.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Configuration.Contracts
{
    using System.Threading.Tasks;

    public interface IAppEnvironmentManager
    {
        Task ResetDesiredStateAsync();

        Task SetDesiredStateAsync();

        Task SetFirstRunDesiredStateAsync();
    }
}
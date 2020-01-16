// <copyright file="ITemplatePackageManager.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Configuration.Contracts
{
    using System.Threading.Tasks;
    using Endjin.Adr.Cli.Templates;

    public interface ITemplatePackageManager
    {
        Task<TemplatePackageMetaData> InstallLatestAsync(string packageId);
    }
}
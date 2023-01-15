// <copyright file="ITemplatePackageManager.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Endjin.Adr.Cli.Templates;

namespace Endjin.Adr.Cli.Configuration.Contracts;
public interface ITemplatePackageManager
{
    Task<TemplatePackageMetaData> InstallLatestAsync(string packageId);
}
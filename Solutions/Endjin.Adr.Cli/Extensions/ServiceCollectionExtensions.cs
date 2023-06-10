// <copyright file="ServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Endjin.Adr.Cli.Configuration;
using Endjin.Adr.Cli.Configuration.Contracts;
using Endjin.Adr.Cli.Templates;

using Microsoft.Extensions.DependencyInjection;

namespace Endjin.Adr.Cli.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureDependencies(this ServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IAppEnvironment, FileSystemRoamingProfileAppEnvironment>();
        serviceCollection.AddTransient<IAppEnvironmentManager, AppEnvironmentManager>();
        serviceCollection.AddTransient<ITemplatePackageManager, NuGetTemplatePackageManager>();
        serviceCollection.AddTransient<ITemplateSettingsManager, TemplateSettingsManager>();
        serviceCollection.AddTransient<IConfigurationLocator, FileSystemConfigurationLocator>();
    }
}
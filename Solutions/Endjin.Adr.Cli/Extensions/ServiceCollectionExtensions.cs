// <copyright file="ServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Extensions
{
    using Endjin.Adr.Cli.Configuration;
    using Endjin.Adr.Cli.Contracts;
    using Endjin.Adr.Cli.Templates;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static void ConfigureDependencies(this ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAppEnvironment, FileSystemLocalProfileAppEnvironment>();
            serviceCollection.AddTransient<IAppEnvironmentManager, AppEnvironmentManager>();
            serviceCollection.AddTransient<ITemplatePackageManager, NuGetTemplatePackageManager>();
            serviceCollection.AddTransient<ITemplateSettingsMananger, TemplateSettingsMananger>();
        }
    }
}
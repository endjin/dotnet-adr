﻿// <copyright file="ServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Extensions
{
    using Endjin.Adr.Cli.Commands;
    using Endjin.Adr.Cli.Commands.Templates;
    using Endjin.Adr.Cli.Commands.Templates.Package;
    using Endjin.Adr.Cli.Configuration;
    using Endjin.Adr.Cli.Configuration.Contracts;
    using Endjin.Adr.Cli.Templates;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static void ConfigureDependencies(this ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAppEnvironment, FileSystemRoamingProfileAppEnvironment>();
            serviceCollection.AddTransient<IAppEnvironmentManager, AppEnvironmentManager>();
            serviceCollection.AddTransient<ITemplatePackageManager, NuGetTemplatePackageManager>();
            serviceCollection.AddTransient<ITemplateSettingsManager, TemplateSettingsManager>();

            // serviceCollection.AddTransient<ICommandFactory<InitCommandFactory>, InitCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<TemplatesCommandFactory>, TemplatesCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<TemplatesPackageCommandFactory>, TemplatesPackageCommandFactory>();
        }
    }
}
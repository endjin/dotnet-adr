// <copyright file="ServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Extensions
{
    using Endjin.Adr.Cli.Commands;
    using Endjin.Adr.Cli.Commands.Environment;
    using Endjin.Adr.Cli.Commands.Environment.Reset;
    using Endjin.Adr.Cli.Commands.Init;
    using Endjin.Adr.Cli.Commands.New;
    using Endjin.Adr.Cli.Commands.Templates;
    using Endjin.Adr.Cli.Commands.Templates.Default;
    using Endjin.Adr.Cli.Commands.Templates.List;
    using Endjin.Adr.Cli.Commands.Templates.Package;
    using Endjin.Adr.Cli.Commands.Templates.Update;
    using Endjin.Adr.Cli.Configuration;
    using Endjin.Adr.Cli.Configuration.Contracts;
    using Endjin.Adr.Cli.Templates;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static void ConfigureDependencies(this ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAppEnvironment, FileSystemLocalProfileAppEnvironment>();
            serviceCollection.AddTransient<IAppEnvironmentManager, AppEnvironmentManager>();
            serviceCollection.AddTransient<ITemplatePackageManager, NuGetTemplatePackageManager>();
            serviceCollection.AddTransient<ITemplateSettingsManager, TemplateSettingsManager>();

            serviceCollection.AddTransient<ICommandFactory<InitCommandFactory>, InitCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<EnvironmentCommandFactory>, EnvironmentCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<NewCommandFactory>, NewCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<TemplatesCommandFactory>, TemplatesCommandFactory>();

            serviceCollection.AddTransient<ICommandFactory<EnvironmentResetCommandFactory>, EnvironmentResetCommandFactory>();

            serviceCollection.AddTransient<ICommandFactory<TemplatesDefaultCommandFactory>, TemplatesDefaultCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<TemplatesDefaultShowCommandFactory>, TemplatesDefaultShowCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<TemplatesDefaultSetCommandFactory>, TemplatesDefaultSetCommandFactory>();

            serviceCollection.AddTransient<ICommandFactory<TemplatesListCommandFactory>, TemplatesListCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<TemplatesUpdateCommandFactory>, TemplatesUpdateCommandFactory>();

            serviceCollection.AddTransient<ICommandFactory<TemplatesPackageCommandFactory>, TemplatesPackageCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<TemplatesPackageSetCommandFactory>, TemplatesPackageSetCommandFactory>();
            serviceCollection.AddTransient<ICommandFactory<TemplatesPackageShowCommandFactory>, TemplatesPackageShowCommandFactory>();
        }
    }
}
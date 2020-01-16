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

            serviceCollection.AddTransient<ICommandFactory<InitCommand>, InitCommand>();
            serviceCollection.AddTransient<ICommandFactory<EnvironmentCommand>, EnvironmentCommand>();
            serviceCollection.AddTransient<ICommandFactory<NewCommand>, NewCommand>();
            serviceCollection.AddTransient<ICommandFactory<TemplatesCommand>, TemplatesCommand>();

            serviceCollection.AddTransient<ICommandFactory<EnvironmentResetCommand>, EnvironmentResetCommand>();

            serviceCollection.AddTransient<ICommandFactory<TemplatesDefaultCommand>, TemplatesDefaultCommand>();
            serviceCollection.AddTransient<ICommandFactory<TemplatesDefaultShowCommand>, TemplatesDefaultShowCommand>();
            serviceCollection.AddTransient<ICommandFactory<TemplatesDefaultSetCommand>, TemplatesDefaultSetCommand>();

            serviceCollection.AddTransient<ICommandFactory<TemplatesListCommand>, TemplatesListCommand>();
            serviceCollection.AddTransient<ICommandFactory<TemplatesUpdateCommand>, TemplatesUpdateCommand>();

            serviceCollection.AddTransient<ICommandFactory<TemplatesPackageCommand>, TemplatesPackageCommand>();
            serviceCollection.AddTransient<ICommandFactory<TemplatesPackageSetCommand>, TemplatesPackageSetCommand>();
            serviceCollection.AddTransient<ICommandFactory<TemplatesPackageShowCommand>, TemplatesPackageShowCommand>();
        }
    }
}
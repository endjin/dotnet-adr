// <copyright file="TemplatesPackageCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Templates.Package
{
    using System.CommandLine;

    public class TemplatesPackageCommandFactory : ICommandFactory<TemplatesPackageCommandFactory>
    {
        private readonly ICommandFactory<TemplatesPackageSetCommandFactory> templatesPackageSetCommandFactory;
        private readonly ICommandFactory<TemplatesPackageShowCommandFactory> templatesPackageShowCommandFactory;

        public TemplatesPackageCommandFactory(
            ICommandFactory<TemplatesPackageSetCommandFactory> templatesPackageSetCommandFactory,
            ICommandFactory<TemplatesPackageShowCommandFactory> templatesPackageShowCommandFactory)
        {
            this.templatesPackageSetCommandFactory = templatesPackageSetCommandFactory;
            this.templatesPackageShowCommandFactory = templatesPackageShowCommandFactory;
        }

        public Command Create()
        {
            var cmd = new Command("package", "Operations that can be performed against the NuGet ADR Template package.");

            cmd.AddCommand(this.templatesPackageSetCommandFactory.Create());
            cmd.AddCommand(this.templatesPackageShowCommandFactory.Create());

            return cmd;
        }
    }
}
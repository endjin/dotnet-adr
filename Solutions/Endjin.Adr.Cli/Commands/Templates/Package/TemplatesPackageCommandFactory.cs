// <copyright file="TemplatesPackageCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Templates.Package
{
    using System.CommandLine;

    public class TemplatesPackageCommandFactory : ICommandFactory<TemplatesPackageCommandFactory>
    {
        private readonly ICommandFactory<TemplatesPackageSetCommandFactory> templatesPackageSetCommand;
        private readonly ICommandFactory<TemplatesPackageShowCommandFactory> templatesPackageShowCommand;

        public TemplatesPackageCommandFactory(ICommandFactory<TemplatesPackageSetCommandFactory> templatesPackageSetCommand, ICommandFactory<TemplatesPackageShowCommandFactory> templatesPackageShowCommand)
        {
            this.templatesPackageSetCommand = templatesPackageSetCommand;
            this.templatesPackageShowCommand = templatesPackageShowCommand;
        }

        public Command Create()
        {
            var cmd = new Command("package", "Operations that can be performed against the NuGet ADR Template package.");

            cmd.AddCommand(this.templatesPackageSetCommand.Create());
            cmd.AddCommand(this.templatesPackageShowCommand.Create());

            return cmd;
        }
    }
}
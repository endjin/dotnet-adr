// <copyright file="TemplatesPackageCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Templates.Package
{
    using System.CommandLine;

    public class TemplatesPackageCommand : ICommandFactory<TemplatesPackageCommand>
    {
        private readonly ICommandFactory<TemplatesPackageSetCommand> templatesPackageSetCommand;
        private readonly ICommandFactory<TemplatesPackageShowCommand> templatesPackageShowCommand;

        public TemplatesPackageCommand(ICommandFactory<TemplatesPackageSetCommand> templatesPackageSetCommand, ICommandFactory<TemplatesPackageShowCommand> templatesPackageShowCommand)
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
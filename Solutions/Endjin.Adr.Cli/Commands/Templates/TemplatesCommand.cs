// <copyright file="TemplatesCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Templates
{
    using System.CommandLine;
    using Endjin.Adr.Cli.Commands.Templates.Default;
    using Endjin.Adr.Cli.Commands.Templates.List;
    using Endjin.Adr.Cli.Commands.Templates.Package;
    using Endjin.Adr.Cli.Commands.Templates.Update;

    public class TemplatesCommand : ICommandFactory<TemplatesCommand>
    {
        private readonly ICommandFactory<TemplatesDefaultCommand> templatesDefaultCommand;
        private readonly ICommandFactory<TemplatesListCommand> templatesListCommand;
        private readonly ICommandFactory<TemplatesUpdateCommand> templatesUpdateCommand;
        private readonly ICommandFactory<TemplatesPackageCommand> templatesPackageCommand;

        public TemplatesCommand(
            ICommandFactory<TemplatesDefaultCommand> templatesDefaultCommand,
            ICommandFactory<TemplatesListCommand> templatesListCommand,
            ICommandFactory<TemplatesUpdateCommand> templatesUpdateCommand,
            ICommandFactory<TemplatesPackageCommand> templatesPackageCommand)
        {
            this.templatesDefaultCommand = templatesDefaultCommand;
            this.templatesListCommand = templatesListCommand;
            this.templatesUpdateCommand = templatesUpdateCommand;
            this.templatesPackageCommand = templatesPackageCommand;
        }

        public Command Create()
        {
            var cmd = new Command("templates", "Perform operations on ADR templates.");

            cmd.AddCommand(this.templatesDefaultCommand.Create());
            cmd.AddCommand(this.templatesListCommand.Create());
            cmd.AddCommand(this.templatesUpdateCommand.Create());
            cmd.AddCommand(this.templatesPackageCommand.Create());

            return cmd;
        }
     }
 }
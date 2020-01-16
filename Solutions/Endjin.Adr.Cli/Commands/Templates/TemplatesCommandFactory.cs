// <copyright file="TemplatesCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Templates
{
    using System.CommandLine;
    using Endjin.Adr.Cli.Commands.Templates.Default;
    using Endjin.Adr.Cli.Commands.Templates.List;
    using Endjin.Adr.Cli.Commands.Templates.Package;
    using Endjin.Adr.Cli.Commands.Templates.Update;

    public class TemplatesCommandFactory : ICommandFactory<TemplatesCommandFactory>
    {
        private readonly ICommandFactory<TemplatesDefaultCommandFactory> templatesDefaultCommand;
        private readonly ICommandFactory<TemplatesListCommandFactory> templatesListCommand;
        private readonly ICommandFactory<TemplatesUpdateCommandFactory> templatesUpdateCommand;
        private readonly ICommandFactory<TemplatesPackageCommandFactory> templatesPackageCommand;

        public TemplatesCommandFactory(
            ICommandFactory<TemplatesDefaultCommandFactory> templatesDefaultCommand,
            ICommandFactory<TemplatesListCommandFactory> templatesListCommand,
            ICommandFactory<TemplatesUpdateCommandFactory> templatesUpdateCommand,
            ICommandFactory<TemplatesPackageCommandFactory> templatesPackageCommand)
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
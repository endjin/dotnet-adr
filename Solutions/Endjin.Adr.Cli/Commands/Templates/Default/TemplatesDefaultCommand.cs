// <copyright file="TemplatesDefaultCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Templates.Default
{
    using System.CommandLine;

    public class TemplatesDefaultCommand : ICommandFactory<TemplatesDefaultCommand>
    {
        private readonly ICommandFactory<TemplatesDefaultShowCommand> templatesDefaultShowCommand;
        private readonly ICommandFactory<TemplatesDefaultSetCommand> templatesDefaultSetCommand;

        public TemplatesDefaultCommand(
            ICommandFactory<TemplatesDefaultShowCommand> templatesDefaultShowCommand,
            ICommandFactory<TemplatesDefaultSetCommand> templatesDefaultSetCommand)
        {
            this.templatesDefaultShowCommand = templatesDefaultShowCommand;
            this.templatesDefaultSetCommand = templatesDefaultSetCommand;
        }

        public Command Create()
        {
            var cmd = new Command("default", "Operations that can be performed against the default ADR templates.");

            cmd.AddCommand(this.templatesDefaultSetCommand.Create());
            cmd.AddCommand(this.templatesDefaultShowCommand.Create());

            return cmd;
        }
    }
}
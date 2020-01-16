// <copyright file="TemplatesDefaultCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Templates.Default
{
    using System.CommandLine;

    public class TemplatesDefaultCommandFactory : ICommandFactory<TemplatesDefaultCommandFactory>
    {
        private readonly ICommandFactory<TemplatesDefaultShowCommandFactory> templatesDefaultShowCommand;
        private readonly ICommandFactory<TemplatesDefaultSetCommandFactory> templatesDefaultSetCommand;

        public TemplatesDefaultCommandFactory(
            ICommandFactory<TemplatesDefaultShowCommandFactory> templatesDefaultShowCommand,
            ICommandFactory<TemplatesDefaultSetCommandFactory> templatesDefaultSetCommand)
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
// <copyright file="TemplatesDefaultCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Templates.Default
{
    using System.CommandLine;

    public class TemplatesDefaultCommandFactory : ICommandFactory<TemplatesDefaultCommandFactory>
    {
        private readonly ICommandFactory<TemplatesDefaultShowCommandFactory> templatesDefaultShowCommandFactory;
        private readonly ICommandFactory<TemplatesDefaultSetCommandFactory> templatesDefaultSetCommandFactory;

        public TemplatesDefaultCommandFactory(
            ICommandFactory<TemplatesDefaultShowCommandFactory> templatesDefaultShowCommandFactory,
            ICommandFactory<TemplatesDefaultSetCommandFactory> templatesDefaultSetCommandFactory)
        {
            this.templatesDefaultShowCommandFactory = templatesDefaultShowCommandFactory;
            this.templatesDefaultSetCommandFactory = templatesDefaultSetCommandFactory;
        }

        public Command Create()
        {
            var cmd = new Command("default", "Operations that can be performed against the default ADR templates.");

            cmd.AddCommand(this.templatesDefaultSetCommandFactory.Create());
            cmd.AddCommand(this.templatesDefaultShowCommandFactory.Create());

            return cmd;
        }
    }
}
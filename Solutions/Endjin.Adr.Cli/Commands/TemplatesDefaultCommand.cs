// <copyright file="TemplatesDefaultCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands
{
    using System.CommandLine;
    using Endjin.Adr.Cli.Contracts;

    public class TemplatesDefaultCommand
    {
        private readonly ITemplateSettingsMananger templateSettingsMananger;

        public TemplatesDefaultCommand(ITemplateSettingsMananger templateSettingsMananger)
        {
            this.templateSettingsMananger = templateSettingsMananger;
        }

        public Command Create()
        {
            var cmd = new Command("default", "Operations that can be performed against the default ADR templates.");

            cmd.AddCommand(new TemplatesDefaultSetCommand(this.templateSettingsMananger).Create());
            cmd.AddCommand(new TemplatesDefaultShowCommand(this.templateSettingsMananger).Create());

            return cmd;
        }
    }
}
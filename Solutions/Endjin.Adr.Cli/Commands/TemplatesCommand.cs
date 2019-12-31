// <copyright file="TemplatesCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands
{
    using System.CommandLine;
    using Endjin.Adr.Cli.Contracts;

    public class TemplatesCommand
    {
        private readonly ITemplateSettingsMananger templateSettingsMananger;

        public TemplatesCommand(ITemplateSettingsMananger templateSettingsMananger)
        {
            this.templateSettingsMananger = templateSettingsMananger;
        }

        public Command Create()
        {
            var cmd = new Command("templates", "Perform operations on ADR templates.");

            cmd.AddCommand(new TemplatesDefaultCommand(this.templateSettingsMananger).Create());
            cmd.AddCommand(new TemplatesInstallCommand().Create());
            cmd.AddCommand(new TemplatesListCommand(this.templateSettingsMananger).Create());
            cmd.AddCommand(new TemplatesUpdateCommand().Create());

            return cmd;
        }
     }
 }
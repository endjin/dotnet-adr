// <copyright file="TemplatesCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands
{
    using System.CommandLine;
    using Endjin.Adr.Cli.Contracts;

    public class TemplatesCommand
    {
        private readonly ITemplatePackageManager templatePackageMananger;
        private readonly ITemplateSettingsMananger templateSettingsMananger;

        public TemplatesCommand(ITemplatePackageManager templatePackageMananger, ITemplateSettingsMananger templateSettingsMananger)
        {
            this.templatePackageMananger = templatePackageMananger;
            this.templateSettingsMananger = templateSettingsMananger;
        }

        public Command Create()
        {
            var cmd = new Command("templates", "Perform operations on ADR templates.");

            cmd.AddCommand(new TemplatesDefaultCommand(this.templateSettingsMananger).Create());
            /*cmd.AddCommand(new TemplatesInstallCommand().Create());*/
            cmd.AddCommand(new TemplatesListCommand(this.templateSettingsMananger).Create());
            cmd.AddCommand(new TemplatesUpdateCommand(this.templatePackageMananger, this.templateSettingsMananger).Create());
            cmd.AddCommand(new TemplatesPackageCommand(this.templateSettingsMananger).Create());

            return cmd;
        }
     }
 }
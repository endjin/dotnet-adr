// <copyright file="TemplatesPackageCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands
{
    using System.CommandLine;
    using Endjin.Adr.Cli.Contracts;

    public class TemplatesPackageCommand
    {
        private readonly ITemplateSettingsMananger templateSettingsMananger;

        public TemplatesPackageCommand(ITemplateSettingsMananger templateSettingsMananger)
        {
            this.templateSettingsMananger = templateSettingsMananger;
        }

        public Command Create()
        {
            var cmd = new Command("package", "Operations that can be performed against the NuGet ADR Template package.");

            cmd.AddCommand(new TemplatesPackageSetCommand(this.templateSettingsMananger).Create());
            cmd.AddCommand(new TemplatesPackageShowCommand(this.templateSettingsMananger).Create());

            return cmd;
        }
    }
}
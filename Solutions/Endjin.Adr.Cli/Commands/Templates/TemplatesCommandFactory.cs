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
        private readonly ICommandFactory<TemplatesDefaultCommandFactory> templatesDefaultCommandFactory;
        private readonly ICommandFactory<TemplatesListCommandFactory> templatesListCommandFactory;
        private readonly ICommandFactory<TemplatesUpdateCommandFactory> templatesUpdateCommand;
        private readonly ICommandFactory<TemplatesPackageCommandFactory> templatesPackageCommandFactory;

        public TemplatesCommandFactory(
            ICommandFactory<TemplatesDefaultCommandFactory> templatesDefaultCommandFactory,
            ICommandFactory<TemplatesListCommandFactory> templatesListCommandFactory,
            ICommandFactory<TemplatesUpdateCommandFactory> templatesUpdateCommandFactory,
            ICommandFactory<TemplatesPackageCommandFactory> templatesPackageCommandFactory)
        {
            this.templatesDefaultCommandFactory = templatesDefaultCommandFactory;
            this.templatesListCommandFactory = templatesListCommandFactory;
            this.templatesUpdateCommand = templatesUpdateCommandFactory;
            this.templatesPackageCommandFactory = templatesPackageCommandFactory;
        }

        public Command Create()
        {
            var cmd = new Command("templates", "Perform operations on ADR templates.");

            cmd.AddCommand(this.templatesDefaultCommandFactory.Create());
            cmd.AddCommand(this.templatesListCommandFactory.Create());
            cmd.AddCommand(this.templatesUpdateCommand.Create());
            cmd.AddCommand(this.templatesPackageCommandFactory.Create());

            return cmd;
        }
     }
 }
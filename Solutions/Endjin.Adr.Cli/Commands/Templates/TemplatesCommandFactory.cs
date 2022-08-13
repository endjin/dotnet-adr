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
        private readonly ICommandFactory<TemplatesPackageCommandFactory> templatesPackageCommandFactory;

        public TemplatesCommandFactory(
            ICommandFactory<TemplatesPackageCommandFactory> templatesPackageCommandFactory)
        {
            this.templatesPackageCommandFactory = templatesPackageCommandFactory;
        }

        public Command Create()
        {
            var cmd = new Command("templates", "Perform operations on ADR templates.");

            cmd.AddCommand(this.templatesPackageCommandFactory.Create());

            return cmd;
        }
     }
 }
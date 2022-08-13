// <copyright file="TemplatesPackageCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Templates.Package
{
    using System.CommandLine;

    public class TemplatesPackageCommandFactory : ICommandFactory<TemplatesPackageCommandFactory>
    {
        public TemplatesPackageCommandFactory()
        {
        }

        public Command Create()
        {
            var cmd = new Command("package", "Operations that can be performed against the NuGet ADR Template package.");

            return cmd;
        }
    }
}
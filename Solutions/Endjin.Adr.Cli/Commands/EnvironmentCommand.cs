// <copyright file="EnvironmentCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands
{
    using System.CommandLine;
    using Endjin.Adr.Cli.Contracts;

    public class EnvironmentCommand
    {
        private readonly IAppEnvironmentManager appEnvironmentManager;

        public EnvironmentCommand(IAppEnvironmentManager appEnvironmentManager2)
        {
            this.appEnvironmentManager = appEnvironmentManager2;
        }

        public Command Create()
        {
            var cmd = new Command("environment", "Manipulate the dotnet-adr environment");

            cmd.AddCommand(new EnvironmentResetCommand(this.appEnvironmentManager).Create());

            return cmd;
        }
    }
}
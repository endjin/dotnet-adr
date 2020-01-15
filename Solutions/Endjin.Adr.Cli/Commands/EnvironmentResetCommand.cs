// <copyright file="EnvironmentResetCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using Endjin.Adr.Cli.Contracts;

    public class EnvironmentResetCommand
    {
        private readonly IAppEnvironmentManager appEnvironmentManager;

        public EnvironmentResetCommand(IAppEnvironmentManager appEnvironmentManager2)
        {
            this.appEnvironmentManager = appEnvironmentManager2;
        }

        public Command Create()
        {
            var cmd = new Command("reset", "Resets the dotnet-adr environment back to its default settings.")
            {
                Handler = CommandHandler.Create(async () =>
                {
                    await this.appEnvironmentManager.ResetDesiredStateAsync();
                }),
            };

            return cmd;
        }
    }
}
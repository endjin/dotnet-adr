// <copyright file="EnvironmentResetCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Environment.Reset
{
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using Endjin.Adr.Cli.Configuration.Contracts;

    public class EnvironmentResetCommandFactory : ICommandFactory<EnvironmentResetCommandFactory>
    {
        private readonly IAppEnvironmentManager appEnvironmentManager;

        public EnvironmentResetCommandFactory(IAppEnvironmentManager appEnvironmentManager)
        {
            this.appEnvironmentManager = appEnvironmentManager;
        }

        public Command Create()
        {
            return new Command("reset", "Resets the dotnet-adr environment back to its default settings.")
            {
                Handler = CommandHandler.Create(async () => await this.appEnvironmentManager.ResetDesiredStateAsync().ConfigureAwait(false)),
            };
        }
    }
}
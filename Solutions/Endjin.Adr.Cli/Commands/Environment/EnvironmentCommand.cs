// <copyright file="EnvironmentCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Environment
{
    using System.CommandLine;
    using Endjin.Adr.Cli.Commands.Environment.Reset;

    public class EnvironmentCommand : ICommandFactory<EnvironmentCommand>
    {
        private readonly ICommandFactory<EnvironmentResetCommand> environmentResetCommand;

        public EnvironmentCommand(ICommandFactory<EnvironmentResetCommand> environmentResetCommand)
        {
            this.environmentResetCommand = environmentResetCommand;
        }

        public Command Create()
        {
            var cmd = new Command("environment", "Manipulate the dotnet-adr environment");

            cmd.AddCommand(this.environmentResetCommand.Create());

            return cmd;
        }
    }
}
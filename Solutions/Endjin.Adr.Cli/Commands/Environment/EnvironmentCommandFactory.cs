// <copyright file="EnvironmentCommandFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Environment
{
    using System.CommandLine;
    using Endjin.Adr.Cli.Commands.Environment.Reset;

    public class EnvironmentCommandFactory : ICommandFactory<EnvironmentCommandFactory>
    {
        private readonly ICommandFactory<EnvironmentResetCommandFactory> environmentResetCommandFactory;

        public EnvironmentCommandFactory(ICommandFactory<EnvironmentResetCommandFactory> environmentResetCommandFactory)
        {
            this.environmentResetCommandFactory = environmentResetCommandFactory;
        }

        public Command Create()
        {
            var cmd = new Command("environment", "Manipulate the dotnet-adr environment.");

            cmd.AddCommand(this.environmentResetCommandFactory.Create());

            return cmd;
        }
    }
}
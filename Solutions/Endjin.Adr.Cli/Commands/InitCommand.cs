// <copyright file="InitCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;

    public class InitCommand
    {
        public Command Create()
        {
            var cmd = new Command("init", "Initialises a new ADR repository.")
            {
                Handler = CommandHandler.Create((string path) =>
                {
                    Console.WriteLine($"Init Repository in {path}");
                }),
            };

            cmd.AddArgument(new Argument<string>("path"));

            return cmd;
        }
    }
}
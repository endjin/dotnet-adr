// <copyright file="NewCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;

    public class NewCommand
    {
        public Command Create()
        {
            var cmd = new Command("new", "Creates a new ADR.")
            {
                Handler = CommandHandler.Create(() =>
                {
                    Console.WriteLine($"Create ADR Record");
                }),
            };

            cmd.AddOption(new Option(new string[] { "supercede", "s" }) { Argument = new Argument<string>("title") });

            return cmd;
        }
    }
}
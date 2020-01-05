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
                Handler = CommandHandler.Create((string id, string title) =>
                {
                    Console.WriteLine($"Supercede ADR Record {id}");
                    Console.WriteLine($"Create ADR Record {title}");
                }),
            };

            cmd.AddOption(new Option("--id", "Id of ADR to supersede.") { Argument = new Argument<string>() });
            cmd.AddArgument(new Argument<string>("title") { Description = "Title of the ADR" });

            return cmd;
        }
    }
}
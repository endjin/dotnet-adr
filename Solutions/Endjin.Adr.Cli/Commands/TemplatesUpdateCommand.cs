// <copyright file="TemplatesUpdateCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;

    public class TemplatesUpdateCommand
    {
        public Command Create()
        {
            var cmd = new Command("update", "Updates to the latest version of the ADR Templates Package.")
            {
                Handler = CommandHandler.Create(() =>
                {
                    Console.WriteLine("Update Templates...");
                }),
            };

            return cmd;
        }
    }
}
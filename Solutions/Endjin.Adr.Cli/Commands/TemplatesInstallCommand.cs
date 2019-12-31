// <copyright file="TemplatesInstallCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;

    public class TemplatesInstallCommand
    {
        public Command Create()
        {
            var cmd = new Command("install", "Install the specificed version of the ADR Templates Package.")
            {
                Handler = CommandHandler.Create(() =>
                {
                    Console.WriteLine("Install Templates...");
                }),
            };

            return cmd;
        }
    }
}
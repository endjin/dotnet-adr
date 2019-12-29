// <copyright file="UpdateTemplatesHandler.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;

    public class UpdateTemplatesHandler
    {
        public Command Create()
        {
            var cmd = new Command("update", "Updates the ADR templates")
            {
                Handler = CommandHandler.Create(() =>
                {
                    Console.WriteLine("Template Name");
                }),
            };

            return cmd;
        }
    }
}
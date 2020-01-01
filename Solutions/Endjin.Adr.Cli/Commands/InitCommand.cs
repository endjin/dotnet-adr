// <copyright file="InitCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.IO;

    public class InitCommand
    {
        public Command Create()
        {
            var cmd = new Command("init", "Initialises a new ADR repository.")
            {
                Handler = CommandHandler.Create((string path) =>
                {
                    if (string.IsNullOrEmpty(path))
                    {
                        path = Path.Combine(Directory.GetCurrentDirectory(), "docs", "adr");
                    }

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);

                        Console.WriteLine($"Created ADR Repository in '{path}'");
                    }
                    else
                    {
                        Console.WriteLine($"'{path}' already exists.");
                    }
                }),
            };

            cmd.AddArgument(new Argument<string>("path") { Arity = ArgumentArity.ZeroOrOne });

            return cmd;
        }
    }
}
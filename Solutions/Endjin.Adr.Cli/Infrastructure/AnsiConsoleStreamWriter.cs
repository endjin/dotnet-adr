// <copyright file="AnsiConsoleStreamWriter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Infrastructure
{
    using System.CommandLine.IO;

    using Spectre.Console;

    internal sealed class AnsiConsoleStreamWriter : IStandardStreamWriter
    {
        private readonly IAnsiConsole console;

        public AnsiConsoleStreamWriter(IAnsiConsole console)
        {
            this.console = console;
        }

        public void Write(string value)
        {
            this.console.Write(value);
        }
    }
}
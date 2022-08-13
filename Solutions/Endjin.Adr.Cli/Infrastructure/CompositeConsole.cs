// <copyright file="CompositeConsole.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Infrastructure
{
    using System;
    using System.CommandLine.IO;

    using Spectre.Console;
    using Spectre.Console.Rendering;

    internal sealed class CompositeConsole : ICompositeConsole
    {
        private readonly AnsiConsoleStreamWriter standardOut;
        private readonly IStandardStreamWriter standardError;

        public CompositeConsole()
        {
            this.standardOut = new AnsiConsoleStreamWriter(AnsiConsole.Console);
            this.standardError = StandardStreamWriter.Create(Console.Error);
        }

        bool IStandardOut.IsOutputRedirected => Console.IsOutputRedirected;

        bool IStandardError.IsErrorRedirected => Console.IsErrorRedirected;

        bool IStandardIn.IsInputRedirected => Console.IsInputRedirected;

        IStandardStreamWriter IStandardOut.Out => this.standardOut;

        IStandardStreamWriter IStandardError.Error => this.standardError;

        public Profile Profile => AnsiConsole.Console.Profile;

        public IAnsiConsoleCursor Cursor => AnsiConsole.Console.Cursor;

        public IAnsiConsoleInput Input => AnsiConsole.Console.Input;

        public IExclusivityMode ExclusivityMode => AnsiConsole.Console.ExclusivityMode;

        public RenderPipeline Pipeline => AnsiConsole.Console.Pipeline;

        public void Clear(bool home)
        {
            AnsiConsole.Console.Clear(home);
        }

        public void Write(IRenderable renderable)
        {
            AnsiConsole.Console.Write(renderable);
        }
    }
}
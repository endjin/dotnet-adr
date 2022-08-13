// <copyright file="ICompositeConsole.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Infrastructure
{
    using System.CommandLine;

    using Spectre.Console;

    public interface ICompositeConsole : IConsole, IAnsiConsole
    {
    }
}
// <copyright file="EnvironmentInitHandler.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Init
{
    using System.CommandLine.Invocation;
    using System.IO;
    using System.Threading.Tasks;
    using Endjin.Adr.Cli.Abstractions;
    using Endjin.Adr.Cli.Configuration.Contracts;
    using Endjin.Adr.Cli.Infrastructure;

    using Microsoft.Extensions.DependencyInjection;

    using Spectre.Console;

    public static class EnvironmentInitHandler
    {
        public static Task<int> ExecuteAsync(
            IServiceCollection services,
            EnvironmentOptions options,
            ICompositeConsole console,
            IAppEnvironment appEnvironment,
            InvocationContext context = null)
        {
            // await appEnvironment.InitializeAsync(console).ConfigureAwait(false);
            if (string.IsNullOrEmpty(options.Path))
            {
                options.Path = Path.Combine(Directory.GetCurrentDirectory(), "docs", "adr");
            }

            if (!Directory.Exists(options.Path))
            {
                Directory.CreateDirectory(options.Path);

                console.Write(new Text($"Created ADR Repository in '{options.Path}'"));
            }
            else
            {
                console.Write(new Text($"'{options.Path}' already exists."));
            }

            return Task.FromResult(ReturnCodes.Ok);
        }
    }
}
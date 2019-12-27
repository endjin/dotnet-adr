// <copyright file="AdrCli.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using McMaster.Extensions.CommandLineUtils;

    /// <summary>
    /// A CLI tool for creating and manading Architectural Decision Records.
    /// </summary>
    public class AdrCli
    {
        /// <summary>
        /// Main entry point into the application.
        /// </summary>
        /// <param name="args">Command Line Switches.</param>
        /// <returns>Exit Code.</returns>
        public static int Main(string[] args)
        {
            CommandLineApplication app = new CommandLineApplication
            {
                Name = "adr",
                FullName = "dotnet-adr",
                LongVersionGetter = () => new Version(FileVersionInfo.GetVersionInfo(typeof(AdrCli).Assembly.Location).ProductVersion).ToString(),
                ShortVersionGetter = () => typeof(AdrCli).Assembly.GetName().Version.ToString(),
                Description = @"This tool is distributed under the APACHE 2.0 licence and the source code is available on GitHub https://github.com/endjin/dotnet-adr",
            };

            app.HelpOption();

            var optionInit = app.Option("-i|--init <DIRECTORY>", "Initialises the directory of architecture decision records", CommandOptionType.SingleValue)
                                  .IsRequired();

            app.OnExecute(() =>
            {
                var directory = optionInit.Value();

                // ? optionInit.Value()

                // : Directory.GetCurrentDirectory();
                Console.WriteLine($"{directory}!");

                return 0;
            });

            app.OnValidationError(r =>
            {
                if (app.GetOptions().All(o => !o.HasValue()))
                {
                    app.ShowHelp();
                }
                else
                {
                    Console.Error.WriteLine(r.ErrorMessage);
                }
            });

            return app.Execute(args);
        }
    }
}
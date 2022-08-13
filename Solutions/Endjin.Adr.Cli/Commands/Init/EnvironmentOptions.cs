// <copyright file="EnvironmentOptions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Commands.Init
{
    public class EnvironmentOptions
    {
        public EnvironmentOptions(string path)
        {
            this.Path = path;
        }

        public string Path { get; set; }
    }
}
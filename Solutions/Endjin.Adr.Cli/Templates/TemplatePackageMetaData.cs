// <copyright file="TemplatePackageMetaData.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Templates
{
    using System.Collections.Generic;
    using System.IO;

    public class TemplatePackageMetaData
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public string TemplateRepositoryPath { get; set; }

        public List<string> Templates { get; set; } = new List<string>();

        public string Id
        {
            get
            {
                return $"{this.Name}.{this.Version}";
            }
        }

        public string TemplatePath
        {
            get
            {
                return Path.Combine(this.TemplateRepositoryPath, this.Version);
            }
        }
    }
}
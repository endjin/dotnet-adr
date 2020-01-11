// <copyright file="TemplatePackageMetaData.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Templates
{
    using System.Collections.Generic;
    using System.IO;
    using Newtonsoft.Json;

    public class TemplatePackageMetaData
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public string RepositoryPath { get; set; }

        public List<TemplatePackageDetail> Details { get; } = new List<TemplatePackageDetail>();

        [JsonIgnore]
        public List<string> Templates { get; } = new List<string>();

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
                return Path.Combine(this.RepositoryPath, this.Version);
            }
        }
    }
}
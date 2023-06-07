// <copyright file="TemplatePackageMetaData.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Endjin.Adr.Cli.Templates;
public class TemplatePackageMetaData
{
    public string Name { get; set; }

    public string Version { get; set; }

    public string RepositoryPath { get; set; }

    public List<TemplatePackageDetail> Details { get; } = new();

    [JsonIgnore]
    public List<string> Templates { get; } = new();

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
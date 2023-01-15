// <copyright file="TemplateSettings.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Endjin.Adr.Cli.Templates;

namespace Endjin.Adr.Cli.Configuration;
public class TemplateSettings
{
    /// <summary>
    /// Gets or sets the selected ADR template name.
    /// </summary>
    public string DefaultTemplate { get; set; }

    public string DefaultTemplatePackage { get; set; }

    public TemplatePackageMetaData MetaData { get; set; }
}
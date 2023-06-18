// <copyright file="TemplatePackageDetail.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;

namespace Endjin.Adr.Cli.Templates;

[DebuggerDisplay("{Title} - {Version}")]
public class TemplatePackageDetail
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Authors { get; set; } = string.Empty;

    public string License { get; set; } = string.Empty;

    public string Effort { get; set; } = string.Empty;

    public string MoreInfo { get; set; } = string.Empty;

    public Version Version { get; set; }

    public DateTime LastModified { get; set; }

    public bool IsDefault { get; set; }

    public string FullPath { get; set; } = string.Empty;

    public string Id { get; set; } = string.Empty;
}
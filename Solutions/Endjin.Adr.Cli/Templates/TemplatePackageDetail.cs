// <copyright file="TemplatePackageDetail.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Templates
{
    using System;

    public class TemplatePackageDetail
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Authors { get; set; }

        public string Effort { get; set; }

        public string MoreInfo { get; set; }

        public Version Version { get; set; }

        public DateTime LastModified { get; set; }

        public bool IsDefault { get; set; }

        public string FullPath { get; set; }

        public string Id { get; set; }
    }
}
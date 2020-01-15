// <copyright file="Adr.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli
{
    public class Adr
    {
        public int RecordNumber { get; set; }

        public string Path { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string SafeFileName()
        {
            return $"{this.RecordNumber.ToString("D4")}-{this.Title.ToLowerInvariant().Replace(" ", "-")}.md";
        }
    }
}
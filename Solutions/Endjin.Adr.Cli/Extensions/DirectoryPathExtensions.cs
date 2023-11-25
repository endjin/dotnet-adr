// <copyright file="DirectoryPathExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;
using Spectre.IO;

namespace Endjin.Adr.Cli.Extensions;

public static class DirectoryPathExtensions
{
    public static IReadOnlyList<DirectoryPath> ChildrenDirectoriesPath(this DirectoryPath directoryPath)
    {
        DirectoryInfo directoryInfo = new(directoryPath.FullPath);
        DirectoryInfo[] directoriesInfos = directoryInfo.GetDirectories();
        var childrenDirectoriesPath = new List<DirectoryPath>();

        foreach (DirectoryInfo childDirectoryInfo in directoriesInfos)
        {
            childrenDirectoriesPath.Add(new DirectoryPath(childDirectoryInfo.FullName));
        }

        return childrenDirectoriesPath.AsReadOnly();
    }
}
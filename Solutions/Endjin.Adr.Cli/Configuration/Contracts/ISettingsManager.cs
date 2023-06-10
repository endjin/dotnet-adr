// <copyright file="ISettingsManager.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Configuration.Contracts;

public interface ISettingsManager<T>
        where T : class
{
    T LoadSettings(string fileName);

    void SaveSettings(T settings, string fileName);
}
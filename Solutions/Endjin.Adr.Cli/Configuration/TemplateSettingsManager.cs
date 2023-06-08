// <copyright file="TemplateSettingsManager.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Endjin.Adr.Cli.Configuration.Contracts;

namespace Endjin.Adr.Cli.Configuration;

public class TemplateSettingsManager : SettingsManager<TemplateSettings>, ITemplateSettingsManager
{
    public TemplateSettingsManager(IAppEnvironment appEnvironment)
            : base(appEnvironment)
    {
    }
}
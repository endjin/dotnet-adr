// <copyright file="TemplateSettingsManager.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Configuration
{
    using Endjin.Adr.Cli.Configuration.Contracts;

    public class TemplateSettingsManager : SettingsManager<TemplateSettings>, ITemplateSettingsManager
    {
        public TemplateSettingsManager(IAppEnvironment appEnvironment)
            : base(appEnvironment)
        {
        }
    }
}
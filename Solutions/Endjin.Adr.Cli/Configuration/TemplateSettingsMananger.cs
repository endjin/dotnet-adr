// <copyright file="TemplateSettingsMananger.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Configuration
{
    using Endjin.Adr.Cli.Contracts;

    public class TemplateSettingsMananger : SettingsManager<TemplateSettings>, ITemplateSettingsMananger
    {
        public TemplateSettingsMananger(IAppEnvironment appEnvironment)
            : base(appEnvironment)
        {
        }
    }
}
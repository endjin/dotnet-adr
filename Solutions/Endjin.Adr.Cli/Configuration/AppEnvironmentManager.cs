// <copyright file="AppEnvironmentManager.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Configuration
{
    using System.Linq;
    using System.Threading.Tasks;
    using Endjin.Adr.Cli.Contracts;

    public class AppEnvironmentManager : IAppEnvironmentManager
    {
        private readonly IAppEnvironment appEnvironment;
        private readonly ITemplatePackageManager templateManager;
        private readonly ITemplateSettingsMananger templateSettingsMananger;

        public AppEnvironmentManager(IAppEnvironment appEnvironment, ITemplatePackageManager templateManager, ITemplateSettingsMananger templateSettingsMananger)
        {
            this.appEnvironment = appEnvironment;
            this.templateManager = templateManager;
            this.templateSettingsMananger = templateSettingsMananger;
        }

        public async Task SetDesiredStateAsync()
        {
            if (!this.appEnvironment.IsInitialized())
            {
                this.appEnvironment.Initialize();

                var defaultPackageId = "Endjin.Adr.Templates";
                var templateMetaData = await this.templateManager.InstallLatestAsync(defaultPackageId).ConfigureAwait(false);
                var templateSettings = new TemplateSettings
                {
                    MetaData = templateMetaData,
                    DefaultTemplate = templateMetaData.Details.FirstOrDefault(x => x.IsDefault).FullPath,
                    DefaultTemplatePackage = defaultPackageId,
                };

                this.templateSettingsMananger.SaveSettings(templateSettings, nameof(TemplateSettings));
            }
        }
    }
}
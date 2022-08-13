// <copyright file="AppEnvironmentManager.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Configuration
{
  using System.Linq;
  using System.Threading.Tasks;
  using Endjin.Adr.Cli.Configuration.Contracts;
  using Endjin.Adr.Cli.Infrastructure;

  public class AppEnvironmentManager : IAppEnvironmentManager
  {
    private readonly IAppEnvironment appEnvironment;
    private readonly ITemplatePackageManager templateManager;
    private readonly ITemplateSettingsManager templateSettingsManager;
    private readonly ICompositeConsole console;

    public AppEnvironmentManager(IAppEnvironment appEnvironment, ITemplatePackageManager templateManager, ITemplateSettingsManager templateSettingsManager, ICompositeConsole console)
    {
      this.appEnvironment = appEnvironment;
      this.templateManager = templateManager;
      this.templateSettingsManager = templateSettingsManager;
      this.console = console;
    }

    public async Task SetDesiredStateAsync()
    {
      const string defaultPackageId = "adr.templates";

      await this.appEnvironment.InitializeAsync(this.console).ConfigureAwait(false);

      var templateMetaData = await this.templateManager.InstallLatestAsync(defaultPackageId).ConfigureAwait(false);
      var templateSettings = new TemplateSettings
      {
        MetaData = templateMetaData,
        DefaultTemplate = templateMetaData.Details.Find(x => x.IsDefault)?.FullPath,
        DefaultTemplatePackage = defaultPackageId,
      };

      this.templateSettingsManager.SaveSettings(templateSettings, nameof(TemplateSettings));
    }

    public async Task SetFirstRunDesiredStateAsync()
    {
      if (!this.appEnvironment.IsInitialized())
      {
        await this.SetDesiredStateAsync().ConfigureAwait(false);
      }
    }

    public async Task ResetDesiredStateAsync()
    {
      this.appEnvironment.Clean();
      await this.SetDesiredStateAsync().ConfigureAwait(false);
    }
  }
}
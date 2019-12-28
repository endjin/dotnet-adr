// <copyright file="NuGetTemplatePackageManager.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Templates
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Endjin.Adr.Cli.Contracts;
    using NuGet.Common;
    using NuGet.Configuration;
    using NuGet.Frameworks;
    using NuGet.Packaging;
    using NuGet.Packaging.Core;
    using NuGet.Packaging.Signing;
    using NuGet.Protocol.Core.Types;
    using NuGet.Resolver;

    #endregion

    public class NuGetTemplatePackageManager : ITemplatePackageManager
    {
        private readonly IAppEnvironment appEnvironment;

        public NuGetTemplatePackageManager(IAppEnvironment appEnvironment)
        {
            this.appEnvironment = appEnvironment;
        }

        public async Task<TemplatePackageMetaData> InstallLatestAsync()
        {
            return await this.GetLatestTemplatePackage("Endjin.Adr.Templates", "any", this.appEnvironment.TemplatesPath).ConfigureAwait(false);
        }

        private Task GetPackage(string v1, string v2)
        {
            throw new NotImplementedException();
        }

        private async Task<TemplatePackageMetaData> GetLatestTemplatePackage(string packageId, string frameworkVersion, string templateRepositoryPath)
        {
            var nuGetFramework = NuGetFramework.ParseFolder(frameworkVersion);
            var settings = Settings.LoadDefaultSettings(root: null);
            var sourceRepositoryProvider = new SourceRepositoryProvider(new PackageSourceProvider(settings), Repository.Provider.GetCoreV3());

            var templatePackageMetaDataList = new List<TemplatePackageMetaData>();

            using (var cacheContext = new SourceCacheContext())
            {
                var repositories = sourceRepositoryProvider.GetRepositories();
                var availablePackages = new HashSet<SourcePackageDependencyInfo>(PackageIdentityComparer.Default);

                foreach (var sourceRepository in repositories)
                {
                    var dependencyInfoResource = await sourceRepository.GetResourceAsync<DependencyInfoResource>().ConfigureAwait(false);

                    var dependencyInfo = await dependencyInfoResource.ResolvePackages(
                        packageId,
                        nuGetFramework,
                        cacheContext,
                        NullLogger.Instance,
                        CancellationToken.None);

                    if (dependencyInfo == null)
                    {
                        continue;
                    }

                    availablePackages.AddRange(dependencyInfo);
                }

                var resolverContext = new PackageResolverContext(
                    DependencyBehavior.Highest,
                    new[] { packageId },
                    Enumerable.Empty<string>(),
                    Enumerable.Empty<PackageReference>(),
                    Enumerable.Empty<PackageIdentity>(),
                    availablePackages,
                    sourceRepositoryProvider.GetRepositories().Select(s => s.PackageSource),
                    NullLogger.Instance);

                var resolver = new PackageResolver();

                var packagesToInstall = resolver.Resolve(resolverContext, CancellationToken.None).Select(p => availablePackages.Single(x => PackageIdentityComparer.Default.Equals(x, p)));
                var packagePathResolver = new PackagePathResolver(SettingsUtility.GetGlobalPackagesFolder(settings));

                var packageExtractionContext = new PackageExtractionContext(
                    PackageSaveMode.Defaultv3,
                    XmlDocFileSaveMode.None,
                    ClientPolicyContext.GetClientPolicy(settings, NullLogger.Instance),
                    NullLogger.Instance);

                var frameworkReducer = new FrameworkReducer();

                foreach (SourcePackageDependencyInfo packageToInstall in packagesToInstall)
                {
                    PackageReaderBase packageReader;
                    var installedPath = packagePathResolver.GetInstalledPath(packageToInstall);

                    if (installedPath == null)
                    {
                        var downloadResource = await packageToInstall.Source.GetResourceAsync<DownloadResource>(CancellationToken.None).ConfigureAwait(false);

                        var downloadResult = await downloadResource.GetDownloadResourceResultAsync(
                            packageToInstall,
                            new PackageDownloadContext(cacheContext),
                            SettingsUtility.GetGlobalPackagesFolder(settings),
                            NullLogger.Instance,
                            CancellationToken.None).ConfigureAwait(false);

                        await PackageExtractor.ExtractPackageAsync(
                            downloadResult.PackageSource,
                            downloadResult.PackageStream,
                            packagePathResolver,
                            packageExtractionContext,
                            CancellationToken.None).ConfigureAwait(false);

                        packageReader = downloadResult.PackageReader;
                    }
                    else
                    {
                        packageReader = new PackageFolderReader(installedPath);
                    }

                    var templatePackageMetaData = new TemplatePackageMetaData();

                    var identity = await packageReader.GetIdentityAsync(CancellationToken.None).ConfigureAwait(false);
                    var contentItems = packageReader.GetContentItems().ToList();

                    templatePackageMetaData.Name = identity.Id;
                    templatePackageMetaData.Version = identity.Version.OriginalVersion;
                    templatePackageMetaData.TemplateRepositoryPath = templateRepositoryPath;

                    foreach (var contentItem in contentItems)
                    {
                        templatePackageMetaData.Templates.AddRange(contentItem.Items);
                    }

                    var packageFileExtractor = new PackageFileExtractor(
                       templatePackageMetaData.Templates,
                       XmlDocFileSaveMode.None);

                    packageReader.CopyFiles(
                       Path.Join(templatePackageMetaData.TemplateRepositoryPath, templatePackageMetaData.Version),
                       templatePackageMetaData.Templates,
                       packageFileExtractor.ExtractPackageFile,
                       NullLogger.Instance,
                       CancellationToken.None);

                    templatePackageMetaDataList.Add(templatePackageMetaData);
                }
            }

            return templatePackageMetaDataList.FirstOrDefault();
        }
    }
}
// <copyright file="NuGetTemplatePackageManager.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Templates
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Endjin.Adr.Cli.Configuration.Contracts;
    using Microsoft.Toolkit.Parsers.Markdown;
    using Microsoft.Toolkit.Parsers.Markdown.Blocks;
    using NuGet.Common;
    using NuGet.Configuration;
    using NuGet.Frameworks;
    using NuGet.Packaging;
    using NuGet.Packaging.Core;
    using NuGet.Packaging.Signing;
    using NuGet.Protocol.Core.Types;
    using NuGet.Resolver;

    public class NuGetTemplatePackageManager : ITemplatePackageManager
    {
        private readonly IAppEnvironment appEnvironment;

        public NuGetTemplatePackageManager(IAppEnvironment appEnvironment)
        {
            this.appEnvironment = appEnvironment;
        }

        public async Task<TemplatePackageMetaData> InstallLatestAsync(string packageId)
        {
            var templateMetaData = await this.GetLatestTemplatePackage(packageId, "any", this.appEnvironment.TemplatesPath).ConfigureAwait(false);

            templateMetaData.Details.AddRange(await this.GetTemplatePackageDetails(templateMetaData.TemplatePath, templateMetaData.Templates).ConfigureAwait(false));

            return templateMetaData;
        }

        private async Task<List<TemplatePackageDetail>> GetTemplatePackageDetails(string templatePackagePath, List<string> templates)
        {
            var packageDetails = new List<TemplatePackageDetail>();

            MarkdownDocument document = new MarkdownDocument();

            foreach (var template in templates)
            {
                var details = new TemplatePackageDetail
                {
                    Id = template.Split('/')[1],
                    FullPath = Path.GetFullPath(Path.Combine(templatePackagePath, template)),
                };

                document.Parse(await File.ReadAllTextAsync(details.FullPath).ConfigureAwait(false));

                YamlHeaderBlock metadata = document.Blocks.FirstOrDefault(x => x.Type == MarkdownBlockType.YamlHeader) as YamlHeaderBlock;

                if (metadata != null && metadata.Children.TryGetValue("Authors", out string authors))
                {
                    details.Authors = authors;
                }

                if (metadata != null && metadata.Children.TryGetValue("Description", out string description))
                {
                    details.Description = description;
                }

                if (metadata != null && metadata.Children.TryGetValue("Effort", out string effort))
                {
                    details.Effort = effort;
                }

                if (metadata != null && metadata.Children.TryGetValue("Default", out string @default))
                {
                    details.IsDefault = bool.Parse(@default);
                }

                if (metadata != null && metadata.Children.TryGetValue("Last Modified", out string lastModified))
                {
                    details.LastModified = DateTime.Parse(lastModified, CultureInfo.InvariantCulture);
                }

                if (metadata.Children.TryGetValue("More Info", out string moreInfo))
                {
                    details.MoreInfo = moreInfo;
                }

                if (metadata.Children.TryGetValue("Title", out string title))
                {
                    details.Title = title;
                }

                if (metadata.Children.TryGetValue("Version", out string version))
                {
                    details.Version = Version.Parse(version);
                }

                packageDetails.Add(details);
            }

            return packageDetails;
        }

        private async Task<TemplatePackageMetaData> GetLatestTemplatePackage(string packageId, string frameworkVersion, string templateRepositoryPath)
        {
            var nugetFramework = NuGetFramework.ParseFolder(frameworkVersion);
            var settings = Settings.LoadSpecificSettings(root: null, this.appEnvironment.NuGetConfigFilePath);
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
                        nugetFramework,
                        cacheContext,
                        NullLogger.Instance,
                        CancellationToken.None).ConfigureAwait(false);

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

                var packageToInstall = resolver.Resolve(resolverContext, CancellationToken.None).Select(p => availablePackages.Single(x => PackageIdentityComparer.Default.Equals(x, p))).FirstOrDefault();
                var packagePathResolver = new PackagePathResolver(SettingsUtility.GetGlobalPackagesFolder(settings));

                var packageExtractionContext = new PackageExtractionContext(
                    PackageSaveMode.Defaultv3,
                    XmlDocFileSaveMode.None,
                    ClientPolicyContext.GetClientPolicy(settings, NullLogger.Instance),
                    NullLogger.Instance);

                var frameworkReducer = new FrameworkReducer();
                var installedPath = packagePathResolver.GetInstalledPath(packageToInstall);
                PackageReaderBase packageReader;

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

                var identity = await packageReader.GetIdentityAsync(CancellationToken.None).ConfigureAwait(false);
                var templatePackageMetaData = new TemplatePackageMetaData
                {
                    Name = identity.Id,
                    Version = identity.Version.OriginalVersion,
                    RepositoryPath = templateRepositoryPath,
                };

                foreach (var contentItem in packageReader.GetContentItems())
                {
                    templatePackageMetaData.Templates.AddRange(contentItem.Items);
                }

                var packageFileExtractor = new PackageFileExtractor(
                    templatePackageMetaData.Templates,
                    XmlDocFileSaveMode.None);

                packageReader.CopyFiles(
                    Path.Join(templatePackageMetaData.RepositoryPath, templatePackageMetaData.Version),
                    templatePackageMetaData.Templates,
                    packageFileExtractor.ExtractPackageFile,
                    NullLogger.Instance,
                    CancellationToken.None);

                templatePackageMetaDataList.Add(templatePackageMetaData);
            }

            return templatePackageMetaDataList.FirstOrDefault();
        }
    }
}
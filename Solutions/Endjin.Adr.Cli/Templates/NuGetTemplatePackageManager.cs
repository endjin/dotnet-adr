// <copyright file="NuGetTemplatePackageManager.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Endjin.Adr.Cli.Configuration.Contracts;

using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;

using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;

using YamlDotNet.Serialization;

using Settings = NuGet.Configuration.Settings;

namespace Endjin.Adr.Cli.Templates;

public class NuGetTemplatePackageManager : ITemplatePackageManager
{
    private readonly IAppEnvironment appEnvironment;

    public NuGetTemplatePackageManager(IAppEnvironment appEnvironment)
    {
        this.appEnvironment = appEnvironment;
    }

    public async Task<TemplatePackageMetaData> InstallLatestAsync(string packageId)
    {
        TemplatePackageMetaData templateMetaData = await this.GetLatestTemplatePackage(packageId, "any", this.appEnvironment.TemplatesPath.ToString()).ConfigureAwait(false);

        templateMetaData.Details.AddRange(await this.GetTemplatePackageDetails(templateMetaData.TemplatePath, templateMetaData.Templates).ConfigureAwait(false));

        return templateMetaData;
    }

    private async Task<List<TemplatePackageDetail>> GetTemplatePackageDetails(string templatePackagePath, List<string> templates)
    {
        List<TemplatePackageDetail> packageDetails = new();
        MarkdownPipeline pipeline = new MarkdownPipelineBuilder()
                .UseAutoIdentifiers()
                .UseGridTables()
                .UsePipeTables()
                .UseYamlFrontMatter()
                .Build();

        MarkdownDocument document = new();

        foreach (string template in templates)
        {
            TemplatePackageDetail details = new()
            {
                    Id = template.Split('/')[1],
                    FullPath = Path.GetFullPath(Path.Combine(templatePackagePath, template)),
            };

            MarkdownDocument doc = Markdown.Parse(await File.ReadAllTextAsync(details.FullPath).ConfigureAwait(false), pipeline);

            YamlFrontMatterBlock yamlBlock = doc.Descendants<YamlFrontMatterBlock>().FirstOrDefault();

            if (yamlBlock != null)
            {
                string yaml = string.Join(Environment.NewLine, yamlBlock.Lines.Lines.Select(l => l.ToString()).Where(x => !string.IsNullOrEmpty(x)));

                try
                {
                    IDeserializer deserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().IgnoreFields().Build();
                    Dictionary<string, dynamic> frontMatter = deserializer.Deserialize<Dictionary<string, dynamic>>(yaml);

                    if (frontMatter.TryGetValue("Authors", out dynamic authors))
                    {
                        details.Authors = authors;
                    }

                    if (frontMatter.TryGetValue("Description", out dynamic description))
                    {
                        details.Description = description;
                    }

                    if (frontMatter.TryGetValue("License", out dynamic license))
                    {
                        details.License = license;
                    }

                    if (frontMatter.TryGetValue("Effort", out dynamic effort))
                    {
                        details.Effort = effort;
                    }

                    if (frontMatter.TryGetValue("Default", out dynamic @default))
                    {
                        if (bool.TryParse(@default, out bool isDefault))
                        {
                            details.IsDefault = isDefault;
                        }
                    }

                    if (frontMatter.TryGetValue("Last Modified", out dynamic lastModified))
                    {
                        if (DateTime.TryParse(lastModified, out DateTime dateTime))
                        {
                            details.LastModified = dateTime;
                        }
                    }

                    if (frontMatter.TryGetValue("More Info", out dynamic moreInfo))
                    {
                        details.MoreInfo = moreInfo;
                    }

                    if (frontMatter.TryGetValue("Title", out dynamic title))
                    {
                        details.Title = title;
                    }

                    if (frontMatter.TryGetValue("Version", out dynamic version))
                    {
                        details.Version = Version.Parse(version);
                    }
                }
                catch (Exception exception)
                {
                    throw new AggregateException(new InvalidOperationException($"Error parsing {details.FullPath}"), exception);
                }
            }

            packageDetails.Add(details);
        }

        return packageDetails;
    }

    private async Task<TemplatePackageMetaData> GetLatestTemplatePackage(string packageId, string frameworkVersion, string templateRepositoryPath)
    {
        NuGetFramework nugetFramework = NuGetFramework.ParseFolder(frameworkVersion);
        ISettings settings = Settings.LoadSpecificSettings(root: null, this.appEnvironment.NuGetConfigFilePath.ToString());
        SourceRepositoryProvider sourceRepositoryProvider = new(new PackageSourceProvider(settings), Repository.Provider.GetCoreV3());

        List<TemplatePackageMetaData> templatePackageMetaDataList = new();

        using (SourceCacheContext cacheContext = new())
        {
            IEnumerable<SourceRepository> repositories = sourceRepositoryProvider.GetRepositories();
            HashSet<SourcePackageDependencyInfo> availablePackages = new(PackageIdentityComparer.Default);

            foreach (SourceRepository sourceRepository in repositories)
            {
                DependencyInfoResource dependencyInfoResource = await sourceRepository.GetResourceAsync<DependencyInfoResource>().ConfigureAwait(false);

                IEnumerable<SourcePackageDependencyInfo> dependencyInfo = await dependencyInfoResource.ResolvePackages(
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

            PackageResolverContext resolverContext = new(
                    DependencyBehavior.Highest,
                    new[] { packageId },
                    Enumerable.Empty<string>(),
                    Enumerable.Empty<PackageReference>(),
                    Enumerable.Empty<PackageIdentity>(),
                    availablePackages,
                    sourceRepositoryProvider.GetRepositories().Select(s => s.PackageSource),
                    NullLogger.Instance);

            PackageResolver resolver = new();

            SourcePackageDependencyInfo packageToInstall = resolver.Resolve(resolverContext, CancellationToken.None).Select(p => availablePackages.Single(x => PackageIdentityComparer.Default.Equals(x, p))).FirstOrDefault();
            PackagePathResolver packagePathResolver = new(SettingsUtility.GetGlobalPackagesFolder(settings));

            PackageExtractionContext packageExtractionContext = new(
                    PackageSaveMode.Defaultv3,
                    XmlDocFileSaveMode.None,
                    ClientPolicyContext.GetClientPolicy(settings, NullLogger.Instance),
                    NullLogger.Instance);

            FrameworkReducer frameworkReducer = new();
            string installedPath = packagePathResolver.GetInstalledPath(packageToInstall);
            PackageReaderBase packageReader;

            if (installedPath == null)
            {
                DownloadResource downloadResource = await packageToInstall.Source.GetResourceAsync<DownloadResource>(CancellationToken.None).ConfigureAwait(false);

                DownloadResourceResult downloadResult = await downloadResource.GetDownloadResourceResultAsync(
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

            PackageIdentity identity = await packageReader.GetIdentityAsync(CancellationToken.None).ConfigureAwait(false);
            TemplatePackageMetaData templatePackageMetaData = new()
            {
                    Name = identity.Id,
                    Version = identity.Version.OriginalVersion,
                    RepositoryPath = templateRepositoryPath,
            };

            foreach (FrameworkSpecificGroup contentItem in packageReader.GetContentItems())
            {
                templatePackageMetaData.Templates.AddRange(contentItem.Items);
            }

            PackageFileExtractor packageFileExtractor = new(
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
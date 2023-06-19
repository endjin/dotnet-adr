# dotnet adr - Make Future You Thank Past You.

A cross platform .NET Global Tool for [creating and managing Architectural Decision Records (ADR)](https://github.com/endjin/dotnet-adr/).

## TLDR;

Architectural Decision Records (ADRs) are simple Markdown documents used to record technical choices for a project by summarizing the context, the decision, and the consequences. dotnet `adr` is a tool and a bundle of the most common ADR templates you can use to create and maintain ADRs in your solution. 

## Create your own custom ADR Template Package

While we use [MADR](#markdown-architectural-decision-records-madr) as the default template, because it has a nice balance of simplicity and power, it doesn't mean that it's the best template for you, your team, and your organization. First check-out [the different templates](#which-adr-templates-are-available-out-of-the-box) which are available out of the box. If none of these are suitable then it's easy to make your own!

This repo contains an example extensibility "Third Party" ADR template example located in `/Solutions/ThirdParty.Adr.Templates`, this is also available via [nuget.org as thirdparty.adr.templates](https://www.nuget.org/packages/thirdparty.adr.templates/).

There are straightforward conventions for creating a customer ADR template package:

1. Create a folder which matches the name of the template, using [kebab-case](https://en.wikipedia.org/wiki/Letter_case#Kebab_case). i.e. `my-custom-adr-template`
2. Inside that folder create a `template.md` file
3. Add the following front-matter and mandatory headings to `template.md`:

```yaml
---
Title: 
Description: 
Authors: 
Effort: 
More Info: 
Version: 
Last Modified: YYYY-MM-DD HH:MM
---
# Title 

## Status
```

4. Create a `.csproj` file which contains the following properties:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <AssemblyName>thirdparty.adr.templates</AssemblyName>
    <TargetFramework>net6.0</TargetFramework>
    <IncludeBuildOutput>false</IncludeBuildOutput>
    <SuppressDependenciesWhenPacking>true</SuppressDependenciesWhenPacking>
    <!-- https://learn.microsoft.com/en-us/dotnet/core/tutorials/creating-app-with-plugin-support -->
    <EnableDynamicLoading>true</EnableDynamicLoading>
  </PropertyGroup>

  <PropertyGroup>
    <PackageLicenseExpression>Apache-2.0</PackageLicenseExpression>
    <PackageDescription>An example demonstrating how a 3rd Party could create a dotnet-adr template package.</PackageDescription>
    <PackageTags>dotnet-adr dotnet-adr-template architecture tools endjin</PackageTags>
    <PackageReleaseNotes></PackageReleaseNotes>
  </PropertyGroup>

  <ItemGroup>
    <Content Include="my-custom-adr-template\template.md">
      <PackagePath>content\my-custom-adr-template</PackagePath>
      <Pack>true</Pack>
    </Content>
  </ItemGroup>

</Project>
```

5. When the solution is built, a `thirdparty.adr.templates.nupkg` NuGet package will be created. Publish this to nuget.org

To swap between the packages use the following `adr` commands:

`adr templates package set thirdparty.adr.templates`

Next, to download the latest version of 'thirdparty.adr.templates` use the command:

`adr templates install`

To see the currently set default package, use: 

`adr templates package show` 

To see the id of the currently set default template, use:

`adr templates default show`

To revert to the "official" ADT Template Package you can either, reset the environment:

`adr environment reset`

or:

`adr templates package set adr.templates`

Then:

`adr templates install`

## More Information

An extensive readme is available in the [GitHub repository](https://github.com/endjin/dotnet-adr/).
# dotnet adr - Make Future You Thank Past You.

A cross platform .NET Global Tool for creating and managing Architectural Decision Records (ADR).

[![Build Status](https://github.com/endjin/dotnet-adr/actions/workflows/build.yml/badge.svg)](https://github.com/endjin/dotnet-adr/actions/workflows/build.yml/)
[![#](https://img.shields.io/nuget/v/adr.svg)](https://www.nuget.org/packages/adr/) 
[![IMM](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/total?cache=false)](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/total?cache=false)
[![GitHub license](https://img.shields.io/badge/License-Apache%202-blue.svg)](https://raw.githubusercontent.com/endjin/dotnet-adr/master/LICENSE)

## TLDR;

Architectural Decision Records (ADRs) are simple Markdown documents used to record technical choices for a project by summarizing the context, the decision, and the consequences. dotnet `adr` is a tool and a bundle of the most common ADR templates you can use to create and maintain ADRs in your solution. 

Install using:

`dotnet tool install -g adr`

Install the default ADR templates using:

`adr templates package set adr.templates`

`adr templates install`

Create a new ADR using:

`adr new <TITLE>`

## Table of Contents:

- [What are Architectural Decision Records?](#what-are-architectural-decision-records)
- [Why we adopted ADRs](#why-we-adopted-adrs)
- [Why create another ADR tool?](#why-create-another-adr-tool)
- [Getting Started](#getting-started)
  - [Installing dotnet adr](#installing-dotnet-adr)
  - [Using dotnet adr](#using-dotnet-adr)
  - [Configure the default ADR location in your repo](#configure-the-default-adr-location-in-your-repo)
- [ADR Templates and ADR Template Packages](#adr-templates-and-adr-template-packages)
  - [Example ADRs](#example-adrs)
  - [Which ADR templates are available out of the box?](#which-adr-templates-are-available-out-of-the-box)
    - [Alexandrian Pattern](#alexandrian-pattern)
    - [Business Case Pattern](#business-case-pattern)
    - [Markdown Architectural Decision Records (MADR)](#markdown-architectural-decision-records-madr)
    - [Merson Pattern](#merson-pattern)
    - [Nygard Pattern](#nygard-pattern)
    - [Planguage Pattern](#planguage-pattern)
    - [Tyree and Akerman Pattern](#tyree-and-akerman-pattern)
  - [ADR Templates and ADR Template Packages](#adr-templates-and-adr-template-packages-1)
  - [Create your own custom ADR Template Package](#create-your-own-custom-adr-template-package)
- [Local System Details](#local-system-details)
- [DevOps](#devops)
- [Packages](#packages)
- [Community Contributions](#community-contributions)
- [Licenses](#licenses)
- [Project Sponsor](#project-sponsor)
- [Acknowledgements](#acknowledgements)
- [Code of conduct](#code-of-conduct)
- [IP Maturity Model (IMM)](#ip-maturity-model-imm)
- [IP Maturity Model Scores](#ip-maturity-model-scores)

## What are Architectural Decision Records?

Context drives intent, which manifests as code. This is the socio-technical contract of modern software development. If over time, code is the only remaining archeological artefact, we are simply left with *effect* without knowing the *cause*. 

Over the last decade we have found immense value in [Gherkin](https://specflow.org/learn/gherkin/) based [Executable Specifications](https://gojko.net/books/specification-by-example/) to describe the behaviour (or intent) of a system; in fact the Gherkin ([Specflow](https://specflow.org/)) feature files have often outlived the original code and have been used to re-implement the system using a more modern language or framework.

Now we have the code, and the intent, but we're still missing an artefact that captures the context. Architectural Decision Records (ADRs) fill this requirement exceedingly well. ADRs are simple text documents (our preferred format is Markdown) which [précis](https://www.merriam-webster.com/dictionary/pr%C3%A9cis) some or all of the following aspects of a decision: 

- Context
- Assumptions
- Rationale
- Decision
- Consequences

The 1st [Law of Simplicity](http://lawsofsimplicity.com/) is "Reduce", and much like Bezos' infamous [6-page memo format](https://www.cnbc.com/2018/04/23/what-jeff-bezos-learned-from-requiring-6-page-memos-at-amazon.html), brevity is the key to the power of ADRs. Rather than a heavyweight functional specification, ADRs have much more in common with minutes from a meeting. The meeting may take hours, but reading the minutes, should take... minutes. 

Good code comments don't explain what the code does, they explain what the developer was thinking when they wrote the code, what assumptions they were making, and what they were trying to achieve. This allows anyone reviewing the code to spot any faults with logic, assumptions that proved to be incorrect, or requirements which have evolved. ADRs operate on the same principle. Anyone can review the ADRs and quickly grok the context, the assumptions, the rationale, the decision, and the consequences, without being bogged down in detail.

With modern cloud native solutions, recording context takes on a nuanced significance; cloud services vary from IaaS to PaaS to SaaS, as a consumer you are not in control of the feature set, the scale characteristics, or the price point. When making an architectural decision you may be constrained by a missing feature, a financial budget, or a performance target. The speed of cloud innovation means that any of these constraints can change on a monthly basis. [Keeping track](https://azureweekly.info) of [feature announcements](https://powerbiweekly.info) and re-evaluating the context of previously made decisions is a engineering practice you should adopt as part of the ADR process.  

The principles of ADRs are straightforward, but the implementation can be simple or as complex as your team or organization requires. The out-of-the-box default template is the [Nygard Pattern](#nygard-pattern), but this tool and repo contains a number of alternative [templates](#which-adr-templates-are-available-out-of-the-box) you can choose from, or you can [create your own](#create-your-own-custom-adr-template-package). If you want to read some real ADRs, check out these [examples](#example-adrs) from our own OSS projects.

We find ADRs to be most effective when they are co-located with the code, in the same repo. We've worked on projects where "all documentation lives in Wiki / Confluence / SharePoint" because not all stakeholders have access to source control repos, but we find this approach to be high friction for all parties involved. This tool, `adr`, is designed to encapsulate our recommended practices. 

## Why we adopted ADRs

Several years ago we worked on a very complex project which required R&D, technical spikes, benchmarking, load-testing, performance tuning cycles, and further benchmarking in order to find the optimal solution. This process worked incredibly well and we delivered orders of magnitude performance improvements over the existing solution. 

During the end-of-project retrospective we identified two sub-optimal outcomes; firstly we felt that while we had worked minor miracles in our technical solutions, the customer never seemed particularly impressed by the improvements. Secondly, the customer hired a new architect just as we finished delivering the solution, and as part of their onboarding process reviewed the solution and criticized almost every aspect with "I wouldn't have done it that way. At my last job we approached it like X and found it to be best". 

We realized that the two issues were related. For the first problem, we concluded that we had failed to follow the most basic instruction you're given at school; "show your workings". We had hidden all the hard work, all the complexity, all of the hypothesis-testing experiments, and just presented the results _Fait Accompli_. The second problem had the same root cause. Because we had not recorded all of our experiments in a systematic way, we had no evidence to show that the approach the architect considered "best" was actually the first approach we took, but when we benchmarked and load-tested the approach it couldn't handle the data throughput at the price-point required by the customer. "Best" is entirely based on situational context. What's "best" in one situation is inappropriate in another.

We embrace evidence-based-decision-making as part of our experimental approach, and wanted to find a process that would allow us to document this in a formalized way. We did some research and discovered Architectural Decision Records. They have now become a fundamental part of our software and data engineering processes.

As a fully-remote organization, a secondary benefit from adopting ADRs has been how it allows us to enable distributed and asynchronous evidence gathering, discussions, decision making, and onboarding. This benefit manifests in a number of different ways; firstly, the process of drafting and evolving an ADR as a working group. Secondly, once the ADR reaches its "proposed status" it's very easy for senior decision makers to quickly grok the summary of the decision and provide input. Thirdly, any new contributor can get up to speed by using the collection of ADRs, AKA an Architecture Decision Log (ADL), to understand all the historical decisions that have been made, and most importantly what the situational context was at the point the decisions were made.

## Why create another ADR tool?

One of the reasons for "re-inventing the wheel" with `adr` when there are so many ADR tools already in existence, is that almost all of those existing tools are opinionated to the point of embedding the ADR templates into the tooling. 

Since we adopted ADRs in 2018, we've changed our default template a number of times. Thus, with `adr` we wanted to decouple the tool from the templates, and make use of NuGet content packages as a mechanism to enable the ecosystem to build / use / share their own templates internally (using Azure DevOps or GitHub private package feeds), or publicly using [nuget.org](https://www.nuget.org/packages?q=Tags%3A%22dotnet-adr%22).

## Getting Started

### Installing dotnet adr

`adr` is a [.NET global tool](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools), which means once installed, it's available on the PATH of your machine. 

To install the `adr` global tool use the following command:

`dotnet tool install -g adr`

To install a specific version, use:

`dotnet tool install -g adr --version <version-number>`

To update to the latest version of the tool, use:

`dotnet tool update -g adr`

To uninstall the tool, use:

`dotnet tool uninstall -g adr`

To list all the global tools installed on your machine, open a command prompt and type:

`dotnet tool list -g`

### Using dotnet adr

Here is a detailed list of the available `adr` commands:

`adr init <PATH>` - Initializes a new Architecture Knowledge Management (AKM) folder. If `<PATH>` is omitted, it will create `docs\adr` in the current directory.

`adr new <TITLE>` - Creates a new Architectural Decision Record, from the current default ADR Template, from the current ADR Template package.

`adr new <TITLE> -i <RECORD NUMBER>` - Creates a new Architectural Decision Record, superseding the specified ADR record, which will have its status updated to reflect this change.

`adr new <TITLE> -p <PATH>` - Creates a new Architectural Decision Record, from the current default ADR Template, from the current ADR Template package, for the Architecture Knowledge Management (AKM) folder located at the specified path.

`adr new <TITLE> -i <RECORD NUMBER> -p <PATH>` - Creates a new Architectural Decision Record, for the Architecture Knowledge Management (AKM) folder located at the specified path, superseding the specified ADR record, which will have its status updated to reflect this change.

`adr templates` - Manipulate ADR Templates & ADR Template Packages. Root command for template operations. Will list available sub-commands.

`adr templates default show` - Displays the detailed metadata of the current default ADR Template.

`adr templates default show --id-only` - Displays the id of the current default ADR Template.

`adr templates default set <TEMPLATE ID>` - Sets the default ADR Template. The `<TEMPLATE ID>` can be obtained from `adr templates default show`

`adr templates list` - Displays a table containing the detailed metadata of all ADR Templates contained in the current default ADR Template Package.

`adr templates list --ids-only` - Displays the ids of all ADR Templates contained in the current default ADR Template Package.

`adr templates list --format-list` - Displays a list of the detailed metadata of all ADR Templates contained in the current default ADR Template Package.

`adr templates install` - Installs the latest version of the currently set ADR Templates Package.

`adr templates update` - Updates to the latest version of the currently set ADR Templates Package.

`adr templates package set <PACKAGE ID>` - Sets the default NuGet ADR Template Package. Use `adr.templates`.

`adr templates package show` - Displays the default NuGet ADR Template Package.

`adr environment` - Manipulate the dotnet-adr environment. Root command for environment operations. Will list available sub-commands.

`adr environment reset` - Resets the `adr` environment back to its default settings.

### Configure the default ADR location in your repo

While `adr` is quite flexible in allowing you to specify were to create or update an ADR, either in the current directory, or by specifying a custom path using `adr new <TITLE> -p <PATH>`, sometime it's better to create a "pit of quality" and standardize the Architecture Knowledge Management (AKM) folder location for all users of the tool.

To support this requirement you can create a file in the root of your repo called `adr.config.json` which must have the following format:

```json
{
    "path": "./Docs/Adr"
}
```

Where the value of `path` is relative to the root of the repo.

## ADR Templates and ADR Template Packages

### Example ADRs

It's always hard to write a document starting from scratch; this is why the default ADR templates contain guidance in the form of headings and notes. Real-world examples are always much more helpful, so below is a list of some publicly available ADRs from our [Open Source projects](https://endjin.com/what-we-do/open-source/). If you explore the repos, you can find more examples:

- [Updating Rx.NET Build for .NET 7.0 era Tooling](https://github.com/dotnet/reactive/blob/main/Rx.NET/Documentation/adr/0001-net7.0-era-tooling-update.md)
- [Implementation of client-side Claims Evaluation](https://github.com/marain-dotnet/Marain.Claims/blob/main/Documentation/ADRs/0001-client-side-claims-evaluation.md)
- [Corvus.Tenancy will not create storage containers automatically](https://github.com/corvus-dotnet/Corvus.Tenancy/blob/main/docs/adr/0003-no-automatic-storage-container-creation.md)
- [Multitargeting .NET Standard 2.0 and 2.1](https://github.com/menes-dotnet/Menes/blob/main/docs/adr/0002-multitargeting-.net-standard-2.0-and-2.1.md)
- [Integration of an Event Store for audit and "change feed" purposes](https://github.com/marain-dotnet/Marain.Workflow/blob/master/docs/adr/0001-integration-of-event-store.md)

### Which ADR templates are available out of the box?

We have collected a number of popular ADR templates.

> NOTE: the status of the Open Source License for some of the templates is unclear. See each template for more details.

#### Alexandrian Pattern
ADR using the Alexandrian [Pattern Language Approach](https://en.wikipedia.org/wiki/Pattern_language) coined by Architect by Christopher Alexander et. al in 1977, which distils the decision record into the following headings:

- Prologue (Summary)
- Discussion (Context)
- Solution (Decision)
- Consequences (Results)

Source [Joel Parker Henderson](https://github.com/joelparkerhenderson/architecture-decision-record/), see this [issue about licensing](https://github.com/joelparkerhenderson/architecture-decision-record/issues/30).

#### Business Case Pattern
Emphasizes creating a business case for a decision, including criteria, candidates, and costs, [created by Joel Parker Henderson](https://github.com/joelparkerhenderson/architecture-decision-record/blob/main/templates/decision-record-template-for-business-case/index.md), which distils the decision record into the following headings:

- Title
- Status
- Evaluation criteria
- Candidates to consider
- Research and analysis of each candidate
  - Does/doesn't meet criteria and why
  - Cost analysis
  - SWOT analysis
  - Opinions and feedback
- Recommendation

Source [Joel Parker Henderson](https://github.com/joelparkerhenderson/architecture-decision-record/), see this [issue about licensing](https://github.com/joelparkerhenderson/architecture-decision-record/issues/30).

#### Markdown Architectural Decision Records (MADR)
Architectural Decisions using Markdown and Architectural Decision Records, by [Oliver Kopp](https://adr.github.io/madr/), which distils the decision record into the following headings:

- Title 
- Context and Problem Statement
- Decision Drivers
- Considered Options
- Decision Outcome
  - Positive Consequences
  - Negative Consequences
- Pros and Cons of the Options
  - [option 1]
  - [option 2]
  - [option 3]
- Links

[Available](https://adr.github.io/madr/) as dual-license under [MIT](https://opensource.org/licenses/MIT) and [CC0](https://creativecommons.org/share-your-work/public-domain/cc0/). You can choose between one of them if you use this work.

#### Merson Pattern
An adaptation of the [Nygard pattern](#nygard-pattern), by [Paulo Merson](https://github.com/pmerson/ADR-template) which adds the rationale behind the decision. It distils the decision record into the following headings:

- Title
- Status
- Decision
- Rationale
- Consequences

[Available](https://github.com/pmerson/ADR-template) under the [MIT License](https://github.com/pmerson/ADR-template/blob/master/LICENSE).

#### Nygard Pattern
A simple, low-friction "Agile" ADR approach by [Michael Nygard](http://thinkrelevance.com/blog/2011/11/15/documenting-architecture-decisions), which distils the decision record into the following headings:

- Title
- Status
- Context
- Decision
- Consequences

[Available](https://cognitect.com/blog/2011/11/15/documenting-architecture-decisions) under [CC0 1.0 Universal (CC0 1.0) Public Domain Dedication](https://creativecommons.org/publicdomain/zero/1.0/).

#### Planguage Pattern
A Quality Assurance oriented approach by [John Terzakis](http://www.iaria.org/conferences2012/filesICCGI12/Tutorial%20Specifying%20Effective%20Non-func.pdf), which distils the decision record into the following headings:

- Title
- Status
- Tag
- Gist
- Requirement
- Rationale
- Priority
- Stakeholders
- Owner
- Author
- Revision
- Date
- Assumptions
- Risks
- Defined

Source [Joel Parker Henderson](https://github.com/joelparkerhenderson/architecture-decision-record/), see this [issue about licensing](https://github.com/joelparkerhenderson/architecture-decision-record/issues/30).

#### Tyree and Akerman Pattern
ADR approach by [Jeff Tyree and Art Akerman](https://personal.utdallas.edu/~chung/SA/zz-Impreso-architecture_decisions-tyree-05.pdf), Capital One Financial, which distils the decision record into the following headings:

- Title
- Status
- Issue
- Decision
- Group
- Assumptions
- Constraints
- Positions
- Argument
- Implications
- Related decisions
- Related requirements
- Related artifacts
- Related principles
- Notes

Source [Joel Parker Henderson](https://github.com/joelparkerhenderson/architecture-decision-record/), see this [issue about licensing](https://github.com/joelparkerhenderson/architecture-decision-record/issues/30).

### ADR Templates and ADR Template Packages

ADR Templates are simply markdown files which contain headings and guidance for the end users. The only hard requirement is that they contains `# Title` and `## Status` headings as `adr` uses Regular Expressions to find and replace these values to power the `adr new <TITLE>` and `adr new -s <RECORD NUMBER> <TITLE>` commands.

The default ADR Templates are contained in the `Endjin.Adr.Templates` project, which contains NuGet configuration elements in `Endjin.Adr.Templates.csproj` to create a NuGet "content" package, which is available via nuget.org as `adr.templates`.

### Create your own custom ADR Template Package

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

## Local System Details

`adr` stores various configuration files and packages in an application profile folder created in:

`%%UserProfile%%\AppData\Roaming\endjin\dotnet-adr`

Templates are stored in:

`templates\`

Configuration in:

`configuration\`

The templates NuGet package is cached in `%%UserProfile%%\.nuget\packages`. 

## DevOps

The project is [hosted on GitHub](https://github.com/endjin/dotnet-adr) and uses a [GitHub Actions workflow](https://github.com/endjin/dotnet-adr/blob/main/.github/workflows/build.yml) and [Endjin.RecommendedPractices.Build](https://www.powershellgallery.com/packages/Endjin.RecommendedPractices.Build/) to manage the full DevOps lifecycle.

## Packages

The NuGet packages for the project, hosted on NuGet.org are:

- [adr](https://www.nuget.org/packages/adr)
- [adr.templates](https://www.nuget.org/packages/adr.templates)
- [thirdparty.adr.templates](https://www.nuget.org/packages/thirdparty.adr.templates)

If you want to [Create your own custom ADR Template Package](#create-your-own-custom-adr-template-package), please add `dotnet-adr` to the `PackageTags` element.

## Community Contributions

- 2021-08-09 - [Christopher Laine](https://medium.com/@domingoladron) wrote a nice blog post about ADRs and dotnet adr: [Architectural Decision Records (ADR) with dotnet-adr](https://medium.com/it-dead-inside/architectural-decision-records-adr-with-dotnet-adr-9fa76104bcd7)

## Licenses

This project is available under the Apache 2.0 open source license.

[![GitHub license](https://img.shields.io/badge/License-Apache%202-blue.svg)](https://raw.githubusercontent.com/endjin/dotnet-adr/master/LICENSE)

For any licensing questions, please email [&#108;&#105;&#99;&#101;&#110;&#115;&#105;&#110;&#103;&#64;&#101;&#110;&#100;&#106;&#105;&#110;&#46;&#99;&#111;&#109;](&#109;&#97;&#105;&#108;&#116;&#111;&#58;&#108;&#105;&#99;&#101;&#110;&#115;&#105;&#110;&#103;&#64;&#101;&#110;&#100;&#106;&#105;&#110;&#46;&#99;&#111;&#109;)

## Project Sponsor

This project is sponsored by [endjin](https://endjin.com), a UK based Technology Consultancy which specializes in Data, AI, DevOps & Cloud, and is a [.NET Foundation Corporate Sponsor](https://dotnetfoundation.org/membership/corporate-sponsorship).

> We help small teams achieve big things.

We produce two free weekly newsletters: 

 - [Azure Weekly](https://azureweekly.info) for all things about the Microsoft Azure Platform
 - [Power BI Weekly](https://powerbiweekly.info) for all things Power BI, Microsoft Fabric, and Azure Synapse Analytics

Keep up with everything that's going on at endjin via our [blog](https://endjin.com/blog), follow us on [Twitter](https://twitter.com/endjin), [YouTube](https://www.youtube.com/c/endjin) or [LinkedIn](https://www.linkedin.com/company/endjin).

We have become the maintainers of a number of popular .NET Open Source Projects:

- [Reactive Extensions for .NET](https://github.com/dotnet/reactive)
- [Reaqtor](https://github.com/reaqtive)
- [Argotic Syndication Framework](https://github.com/argotic-syndication-framework/)

And we have over 50 Open Source projects of our own, spread across the following GitHub Orgs:

- [endjin](https://github.com/endjin/)
- [Corvus](https://github.com/corvus-dotnet)
- [Menes](https://github.com/menes-dotnet)
- [Marain](https://github.com/marain-dotnet)
- [AIS.NET](https://github.com/ais-dotnet)

And the DevOps tooling we have created for managing all these projects is available on the [PowerShell Gallery](https://www.powershellgallery.com/profiles/endjin).

For more information about our products and services, or for commercial support of this project, please [contact us](https://endjin.com/contact-us). 

## Acknowledgements

- [Patrik Svensson](https://twitter.com/firstdrafthell) for his help, support, feature requests, and amazing work on [Spectre.Console](https://github.com/spectreconsole/spectre.console), and [Covenant](https://github.com/patriksvensson/covenant)
- [Joel Parker Henderson](https://github.com/joelparkerhenderson) for [collating the various ADR templates](https://github.com/joelparkerhenderson/architecture_decision_record) we make use of.
- [David Glick](https://daveaglick.com/) for his incredibly [useful blog series](https://daveaglick.com/posts/exploring-the-nuget-v3-libraries-part-1) on the NuGet v3 SDK API.
- [Martin Björkström](https://twitter.com/mholo65) for his [excellent gist](https://gist.github.com/mholo65/ad5776c36559410f45d5dcd0181a5c64) that does the heavy lifting of downloading and extracting NuGet packages.

## Code of conduct

This project has adopted a code of conduct adapted from the [Contributor Covenant](http://contributor-covenant.org/) to clarify expected behavior in our community. This code of conduct has been [adopted by many other projects](http://contributor-covenant.org/adopters/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [&#104;&#101;&#108;&#108;&#111;&#064;&#101;&#110;&#100;&#106;&#105;&#110;&#046;&#099;&#111;&#109;](&#109;&#097;&#105;&#108;&#116;&#111;:&#104;&#101;&#108;&#108;&#111;&#064;&#101;&#110;&#100;&#106;&#105;&#110;&#046;&#099;&#111;&#109;) with any additional questions or comments.

## IP Maturity Model (IMM)

The [IP Maturity Model](https://github.com/endjin/Endjin.Ip.Maturity.Matrix) is endjin's IP quality assessment framework, which we've developed over a number of years when doing due diligence assessments of 3rd party systems. We've codified the approach into a [configurable set of rules](https://github.com/endjin/Endjin.Ip.Maturity.Matrix.RuleDefinitions), which are committed into the [root of a repo](imm.yaml), and a [Azure Function HttpTrigger](https://github.com/endjin/Endjin.Ip.Maturity.Matrix/tree/main/Solutions/Endjin.Ip.Maturity.Matrix.Host) HTTP endpoint which can evaluate the ruleset, and render an svg badge for display in repo's `readme.md`.

## IP Maturity Model Scores

[![Shared Engineering Standards](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/74e29f9b-6dca-4161-8fdd-b468a1eb185d?nocache=true)](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/74e29f9b-6dca-4161-8fdd-b468a1eb185d?cache=false)

[![Coding Standards](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/f6f6490f-9493-4dc3-a674-15584fa951d8?cache=false)](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/f6f6490f-9493-4dc3-a674-15584fa951d8?cache=false)

[![Executable Specifications](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/bb49fb94-6ab5-40c3-a6da-dfd2e9bc4b00?cache=false)](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/bb49fb94-6ab5-40c3-a6da-dfd2e9bc4b00?cache=false)

[![Code Coverage](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/0449cadc-0078-4094-b019-520d75cc6cbb?cache=false)](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/0449cadc-0078-4094-b019-520d75cc6cbb?cache=false)

[![Benchmarks](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/64ed80dc-d354-45a9-9a56-c32437306afa?cache=false)](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/64ed80dc-d354-45a9-9a56-c32437306afa?cache=false)

[![Reference Documentation](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/2a7fc206-d578-41b0-85f6-a28b6b0fec5f?cache=false)](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/2a7fc206-d578-41b0-85f6-a28b6b0fec5f?cache=false)

[![Design & Implementation Documentation](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/f026d5a2-ce1a-4e04-af15-5a35792b164b?cache=false)](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/f026d5a2-ce1a-4e04-af15-5a35792b164b?cache=false)

[![How-to Documentation](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/145f2e3d-bb05-4ced-989b-7fb218fc6705?cache=false)](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/145f2e3d-bb05-4ced-989b-7fb218fc6705?cache=false)

[![Date of Last IP Review](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/da4ed776-0365-4d8a-a297-c4e91a14d646?cache=false)](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/da4ed776-0365-4d8a-a297-c4e91a14d646?cache=false)

[![Framework Version](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/6c0402b3-f0e3-4bd7-83fe-04bb6dca7924?cache=false)](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/6c0402b3-f0e3-4bd7-83fe-04bb6dca7924?cache=false)

[![Associated Work Items](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/79b8ff50-7378-4f29-b07c-bcd80746bfd4?cache=false)](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/79b8ff50-7378-4f29-b07c-bcd80746bfd4?cache=false)

[![Source Code Availability](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/30e1b40b-b27d-4631-b38d-3172426593ca?cache=false)](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/30e1b40b-b27d-4631-b38d-3172426593ca?cache=false)

[![License](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/d96b5bdc-62c7-47b6-bcc4-de31127c08b7?cache=false)](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/d96b5bdc-62c7-47b6-bcc4-de31127c08b7?cache=false)

[![Production Use](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/87ee2c3e-b17a-4939-b969-2c9c034d05d7?cache=false)](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/87ee2c3e-b17a-4939-b969-2c9c034d05d7?cache=false)

[![Insights](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/71a02488-2dc9-4d25-94fa-8c2346169f8b?cache=false)](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/71a02488-2dc9-4d25-94fa-8c2346169f8b?cache=false)

[![Packaging](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/547fd9f5-9caf-449f-82d9-4fba9e7ce13a?cache=false)](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/547fd9f5-9caf-449f-82d9-4fba9e7ce13a?cache=false)

[![Deployment](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/edea4593-d2dd-485b-bc1b-aaaf18f098f9?cache=false)](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/edea4593-d2dd-485b-bc1b-aaaf18f098f9?cache=false)

[![OpenChain](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/66efac1a-662c-40cf-b4ec-8b34c29e9fd7?cache=false)](https://imm.endjin.com/api/imm/github/endjin/dotnet-adr/rule/66efac1a-662c-40cf-b4ec-8b34c29e9fd7?cache=false)
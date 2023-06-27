<#
.SYNOPSIS
    Runs a .NET flavoured build process.
.DESCRIPTION
    This script was scaffolded using a template from the Endjin.RecommendedPractices.Build PowerShell module.
    It uses the InvokeBuild module to orchestrate an opinonated software build process for .NET solutions.
.EXAMPLE
    PS C:\> ./build.ps1
    Downloads any missing module dependencies (Endjin.RecommendedPractices.Build & InvokeBuild) and executes
    the build process.
.PARAMETER Tasks
    Optionally override the default task executed as the entry-point of the build.
.PARAMETER Configuration
    The build configuration, defaults to 'Release'.
.PARAMETER BuildRepositoryUri
    Optional URI that supports pulling MSBuild logic from a web endpoint (e.g. a GitHub blob).
.PARAMETER SourcesDir
    The path where the source code to be built is located, defaults to the current working directory.
.PARAMETER CoverageDir
    The output path for the test coverage data, if run.
.PARAMETER TestReportTypes
    The test report format that should be generated by the test report generator, if run.
.PARAMETER PackagesDir
    The output path for any packages produced as part of the build.
.PARAMETER LogLevel
    The logging verbosity.
.PARAMETER Clean
    When true, the .NET solution will be cleaned and all output/intermediate folders deleted.
.PARAMETER BuildModulePath
    The path to import the Endjin.RecommendedPractices.Build module from. This is useful when
    testing pre-release versions of the Endjin.RecommendedPractices.Build that are not yet
    available in the PowerShell Gallery.
.PARAMETER BuildModuleVersion
    The version of the Endjin.RecommendedPractices.Build module to import. This is useful when
    testing pre-release versions of the Endjin.RecommendedPractices.Build that are not yet
    available in the PowerShell Gallery.
.PARAMETER InvokeBuildModuleVersion
    The version of the InvokeBuild module to be used.
#>
[CmdletBinding()]
param (
    [Parameter(Position=0)]
    [string[]] $Tasks = @("."),

    [Parameter()]
    [string] $Configuration = "Debug",

    [Parameter()]
    [string] $BuildRepositoryUri = "",

    [Parameter()]
    [string] $SourcesDir = $PWD,

    [Parameter()]
    [string] $CoverageDir = "_codeCoverage",

    [Parameter()]
    [string] $TestReportTypes = "Cobertura",

    [Parameter()]
    [string] $PackagesDir = "_packages",

    [Parameter()]
    [ValidateSet("minimal","normal","detailed")]
    [string] $LogLevel = "minimal",

    [Parameter()]
    [switch] $Clean,

    [Parameter()]
    [string] $BuildModulePath,

    [Parameter()]
    [version] $BuildModuleVersion = "1.3.5",

    [Parameter()]
    [version] $InvokeBuildModuleVersion = "5.7.1"
)

$ErrorActionPreference = $ErrorActionPreference ? $ErrorActionPreference : 'Stop'
$InformationPreference = 'Continue'

$here = Split-Path -Parent $PSCommandPath

#region InvokeBuild setup
if (!(Get-Module -ListAvailable InvokeBuild)) {
    Install-Module InvokeBuild -RequiredVersion $InvokeBuildModuleVersion -Scope CurrentUser -Force -Repository PSGallery
}
Import-Module InvokeBuild
# This handles calling the build engine when this file is run like a normal PowerShell script
# (i.e. avoids the need to have another script to setup the InvokeBuild environment and issue the 'Invoke-Build' command )
if ($MyInvocation.ScriptName -notlike '*Invoke-Build.ps1') {
    try {
        Invoke-Build $Tasks $MyInvocation.MyCommand.Path @PSBoundParameters
    }
    catch {
        $_.ScriptStackTrace
        throw
    }
    return
}
#endregion

#region Import shared tasks and initialise build framework
if (!($BuildModulePath)) {
    if (!(Get-Module -ListAvailable Endjin.RecommendedPractices.Build | ? { $_.Version -eq $BuildModuleVersion })) {
        Write-Information "Installing 'Endjin.RecommendedPractices.Build' module..."
        Install-Module Endjin.RecommendedPractices.Build -RequiredVersion $BuildModuleVersion -Scope CurrentUser -Force -Repository PSGallery
    }
    $BuildModulePath = "Endjin.RecommendedPractices.Build"
}
else {
    Write-Information "BuildModulePath: $BuildModulePath"
}
Import-Module $BuildModulePath -RequiredVersion $BuildModuleVersion -Force

# Load the build process & tasks
. Endjin.RecommendedPractices.Build.tasks
#endregion


#
# Build process control options
#
$SkipInit = $false
$SkipVersion = $false
$SkipBuild = $false
$CleanBuild = $Clean
$SkipTest = $true
$SkipTestReport = $false
$SkipAnalysis = $false
$SkipPackage = $false
$SkipPublish = $false


#
# Build process configuration
#
$SolutionToBuild = (Resolve-Path (Join-Path $here ".\Solutions\Endjin.Adr.sln")).Path

#
# Specify files to exclude from code coverage
# This option is for excluding generated code
# - Use file path or directory path with globbing (e.g dir1/*.cs)
# - Use single or multiple paths (separated by comma) (e.g. **/dir1/class1.cs,**/dir2/*.cs,**/dir3/**/*.cs)
#
$ExcludeFilesFromCodeCoverage = ""

# Synopsis: Build, Test and Package
task . FullBuild


# build extensibility tasks
task RunFirst {}
task PreInit {}
task PostInit {}
task PreVersion {}
task PostVersion {}
task PreBuild {}
task PostBuild {}
task PreTest {}
task PostTest {}
task PreTestReport {}
task PostTestReport {}
task PreAnalysis {}
task PostAnalysis RunSBOMAnalysis
task PrePackage {}
task PostPackage {}
task PrePublish {}
task PostPublish {}
task RunLast {}

task RunSBOMAnalysis {
    if (!(Get-Module Az.Storage -ListAvailable)){
        Install-Module Az.Storage -Scope CurrentUser -Repository PSGallery -Force -Verbose
    }
    
    $AnalysisOutputContainerName = "data"
    $AnalysisOutputStorageAccountName = "endsynapsedatalake"
    # 1. Download JSON ruleset 
    $uri = "https://{0}.blob.core.windows.net/{1}/{2}/{3}" -f $AnalysisOutputStorageAccountName,
                        $AnalysisOutputContainerName,
                        "openchain/license_rules",
                        "license_rule_set.json"

    $authUri = exec {& az storage blob generate-sas `
                            --auth-mode login `
                            --as-user `
                            --https-only `
                            --account-name $AnalysisOutputStorageAccountName `
                            --blob-url $uri `
                            --permissions re `
                            --start (Get-Date).ToUniversalTime().ToString("yyyy-M-d'T'H:m'Z'") `
                            --expiry (Get-Date).AddMinutes(10).ToUniversalTime().ToString("yyyy-M-d'T'H:m'Z'") `
                            --full-uri `
                            -o tsv}
    Write-Host $authUri
    Get-AzStorageBlobContent -absoluteuri $authUri -Force | fl 

    # Find latest version released on GitHub
    $env:GH_TOKEN = $env:CUSTOM_GITHUB_TOKEN
    $latestVersion = exec { gh release list -R endjin/endjin-sbom-analyser --limit 1 } |
                        ConvertFrom-Csv -Header title,type,"tag name",published -Delimiter `t |
                        Select-Object -ExpandProperty "tag name"
    
    if (!$latestVersion) {
        throw "Unable to determine the latest version of the Python tool"
    }

    Write-Host $latestVersion

    $SBOMAnalyserDownloadPath = 'SBOMAnalyser'
    if (!(Test-Path $SBOMAnalyserDownloadPath)) {
        New-Item -ItemType Directory $SBOMAnalyserDownloadPath | Out-Null
    }

    $DownloadFileName = "sbom_analyser-$($latestVersion)-py3-none-any.whl"
    Write-Host "Downloading latest release of SBOM Analyser, version" $latestVersion
    exec { & gh release download -R "endjin/endjin-sbom-analyser" -p $DownloadFileName -D $SBOMAnalyserDownloadPath}

    exec {
        pip install poetry
        pip install (Join-Path $SBOMAnalyserDownloadPath $DownloadFileName)
        $sbomPath = Get-ChildItem -path '*sbom.json'
        Write-Host $sbomPath
        $jsonPath = Get-ChildItem -path 'openchain/license_rules/*.json'
        Write-Host $jsonPath
        generate_sbom_score $sbomPath $jsonPath
    }

    $summarisedContent = Get-Content 'summarised_scores.csv' | ConvertFrom-Csv
    $rejectedComponents = Get-Content 'rejected_components.csv' 

    function Write-Components {

        param (
            $fileName,
            $summarisedContent
        )

        $components = Import-Csv $fileName | Select-Object -Property name, license
        $components = $components | ForEach-Object {"$_`n"}

        $content = "There are $($summarisedContent.Rejected) rejected components in this build, please review the 'rejected_components.csv' and make appropriate changes `n"
        $content = $content + ($components | Out-String)

        return $content

    }

    if ($summarisedContent.Rejected -gt 0){
        $rejectedComponents = Write-Components -fileName 'rejected_components.csv'
        throw Write-Components 'rejected_components.csv' $summarisedContent
    }
    if ($summarisedContent.Unknown -gt 0){
        Write-Warning "There are $($summarisedContent.Unknown) unknown components in this build, please review the 'unknown_components.csv' and make appropriate changes"
    }

    Remove-Item -Recurse 'openchain' -Force
    Remove-Item -Recurse 'SBOMAnalyser' -Force
}
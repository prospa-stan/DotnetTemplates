# <img src="https://raw.githubusercontent.com/prospa-group/DotnetPackaging/master/prospa60x60.png" alt="Prospa Engineering" width="50px"/> Prospa dotnet new Templates 

|Branch|Azure Devops|
|------|:--------:|
|dev|[![Build Status](https://dev.azure.com/prospaoss/dotnet/_apis/build/status/prospa-group-oss.DotnetTemplates?branchName=master)](https://dev.azure.com/prospaoss/dotnet/_build/latest?definitionId=1&branchName=dev)
|master|[![Build Status](https://dev.azure.com/prospaoss/dotnet/_apis/build/status/prospa-group-oss.DotnetTemplates?branchName=master)](https://dev.azure.com/prospaoss/dotnet/_build/latest?definitionId=1&branchName=master)|

## Install from Nuget

```console
dotnet new --install Prospa.Templates::*
```

Confirm templates are installed:

```console
dotnet new list
``` 

## Project Templates Included

|Short Name|Description|
|------|--------|
|[prospaapiazure](docs/AspNetCoreApiAzure.md)|An ASP.NET Core Api targetted at hosting on Azure|
|[prospaapiazurensb](docs/AspNetCoreApiAzureNsb.md)|An ASP.NET Core Api with NServiceBus integration targetted at hosting on Azure|

## How to Build

```csharp
./build.ps1 -Target Local
```

Package will be output to `/.artifacts/packages/Prospa.Templates.{version}.nupkg`

## Test locally

After building the project, the templates can be installed using the following:

### Install

```console
dotnet new -i <path to project>\.artifacts\packages\Prospa.Templates.{version}.nupkg
```

### Re-Install

```console
dotnet new -u Prospa.Templates
dotnet new -i <path to project>\.artifacts\packages\Prospa.Templates.{version}.nupkg
```

### Install location

`%USERPROFILE%\.templateengine\dotnetcli\<SDK version>`

## Versioning

To version bump, edit the `VersionPrefix` and/or `VersionSuffix` in `./version.props`

> When on non-release branches the `VersionSuffix` will always be set to `alpha` with the build number appended.

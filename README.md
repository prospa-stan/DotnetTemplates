# <img src="https://raw.githubusercontent.com/prospa-group/DotnetPackaging/master/prospa60x60.png" alt="Prospa Engineering" width="50px"/> Prospa dotnet new Templates 

## Install from Nuget

```console
dotnet new --install ProspaTemplates::*
```

Confirm templates are installed:

```console
dotnet new list
``` 

## Project Templates Included

### AspNetCore API

#### Create a new solution to add the project template to:

See details [here](https://github.com/prospa-group/DotnetSolution)

#### Create a new AspNetCore API using the template:

```console
cd src
dotnet new prospaapi -n "MyNew.API" 
--metricsHealthEndpointToken {The token allowing access to the metrics and health endpoints} 
--hostedMetricsBaseUri {The Grafana hosted metrics base URI where metrics are flushed} 
--hostedMetricsApiKeyStaging {The Grafana hosted metrics API Key to use for the staging environment} 
--hostedMetricsApiKeyProduction {The Grafana hosted metrics API Key to use for the production environment} 
--slackWebhookUrlStaging {The Slack Webhook URL for health check alerting for the staging environment} 
--slackWebhookUrlProduction {The Slack Webhook URL for health check alerting for the production environment}
```

#### Attach the API to the solution

```console
cd ..
dotnet sln add .\src\MyNew.API\MyNew.API.csproj
```

## How to Build

```csharp
./build.ps1 -Target Local
```

Package will be output to `/.artifacts/packages/ProspaTemplates.{version}.nupkg`

## Test locally

After building the project, the templates can be installed using the following:

### Install

```console
dotnet new -i <path to project>\.artifacts\packages\ProspaTemplates.{version}.nupkg
```

### Re-Install

```console
dotnet new -u ProspaTemplates
dotnet new -i <path to project>\.artifacts\packages\ProspaTemplates.{version}.nupkg
```

### Install location

`%USERPROFILE%\.templateengine\dotnetcli\<SDK version>`

## Versioning

To version bump, edit the `VersionPrefix` and/or `VersionSuffix` in `./version.props`

> When on non-release branches the `VersionSuffix` will always be set to `alpha` with the build number appended.

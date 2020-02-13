### AspNetCore API - Azure

#### Create a new solution to add the project template to:

See details [here](https://github.com/prospa-group/DotnetSolution)

#### Create a new project using the template:

```console
cd src
dotnet new prospaapiazure -n "MyNew.API" 
--keyvaultName {Keyvault name is required, don't include the environment prefix or use the DNS name. e.g. template-keyvault}
```

#### Attach the project to the solution

```console
cd ..
dotnet sln add .\src\MyNew.API\MyNew.API.csproj
```

#### Configure Azure Keyvault

The template is configured to add Azure Keyvault as a configuration provider. You'll need to create the KeyVault resource for the application and use MSI to connect.

1. Install the [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest) and then run `az login`
2. Create a Resource Group
	- `az group create -l australiaeast -n demo-mygroupname-rg`
3. Create a Keyvault Resource using the same name as the Key Vault provided when creating the template proje
	- `az keyvault create -n "keyvaultName" -g "demo-mygroup-rg" -l australiaeast`
4. Set the required secrets in the KeyVault
	- `az keyvault secret set --vault-name "keyvaultName" -n "EndpointKey" --value "secret"`
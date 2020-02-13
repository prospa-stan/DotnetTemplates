### AspNetCore API with NServiceBus Integration - Azure

#### Create a new solution to add the project template to:

See details [here](https://github.com/prospa-group/DotnetSolution)

#### Create a new project using the template:

```console
cd src
dotnet new prospaapiazurensb -n "MyNew.API" 
--keyvaultName {Keyvault name is required, don't include the environment prefix or use the DNS name. e.g. template-keyvault}
```

#### Attach the project to the solution

```console
cd ..
dotnet sln add .\src\MyNew.API\MyNew.API.csproj
```

#### Create an Azure Resource Group

An Azure Resource Group is required to hold Azure Keyvault and Azure Service Bus resources. To create a Resource Group:

1. Install the [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest) and then run `az login`

2. Create the resource group
```
location=australiaeast
rg=demo-mygroupname-rg
az group create -l $location -n $rg
```

#### Configure Azure Service Bus

NServiceBus in the template is configured to use Azure Service Bus. To create an Azure Service Bus Resource create the namespace and keep the connection string to store in Keyvault

```
namespaceName="demo-myservicebusname-sb"
az servicebus namespace create --resource-group $rg --name "demo-myservicebusname-sb" --location $location
serviceBusConnectionString=$(az servicebus namespace authorization-rule keys list --g $rg --namespace-name $namespaceName --name RootManageSharedAccessKey --query primaryConnectionString --output tsv)
```

#### Configure Azure Keyvault

The template is configured to add Azure Keyvault as a configuration provider. You'll need to create the KeyVault resource for the application and use MSI to connect.

```
keyvaultName="mykeyvaultName" # i.e. The name provided on template creation
az keyvault create -n $keyvaultName -g $rg -l $location
az keyvault secret set --vault-name $keyvaultName -n "EndpointKey" --value "secret"
az keyvault secret set --vault-name $keyvaultName -n "ConnectionStrings--ServiceBus" --value $serviceBusConnectionString
```
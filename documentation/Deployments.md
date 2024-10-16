# Deployments

## Deploy to Azure

### Azure app service

- Create webabb via web
  - resourcegroup name `myResourceGroup`
  - name `my-first-api`
  - region `UK South`
- az login
- `az webapp up --name my-first-api --resource-group myResourceGroup --location uksouth`
- `az webapp delete --name my-first-api --resource-group myResourceGroup`
- `az group delete --name myResourceGroup --yes --no-wait`

## Docker

`docker build -t api .`

`docker run -it --rm -p 9999:8080 --name api_example api`

## Deploy to azure container app

### Add image to Azure Container Registry

`az group create --name myResourceGroup --location uksouth`

`az acr create --resource-group myResourceGroup --name <ACR_NAME> --sku Basic`
(the above command takes a while. ACR name must be unique)

`az acr login --name <ACR_NAME>`

`docker tag api <ACR_NAME>.azurecr.io/api:latest`

`docker push <ACR_NAME>.azurecr.io/api:latest`
(the dockerfile for this is huge and needs to be minimised)

### Create azure container app

Container apps are in preview???

Enable the feature
`az extension add --name containerapp`

Creates a container app ENVIRONMENT and not a container app. thats the next step.
`az containerapp env create --name myContainerAppEnv --resource-group myResourceGroup --location uksouth`

Create the container app

`az containerapp create --name myapi --resource-group myResourceGroup --environment myContainerAppEnv --image <ACR_NAME>.azurecr.io/api:latest --target-port 80 --ingress 'external'`

`az containerapp create --name myapi --resource-group myResourceGroup --environment myContainerAppEnv --image intrepidtiger.azurecr.io/api:latest --target-port 80 --ingress 'external'`

this fails due to not being authorised. to Resolve? - Doesnt work

`az acr update -n <ACR_NAME> --admin-enabled true`

https://learn.microsoft.com/en-gb/azure/container-registry/container-registry-authentication?WT.mc_id=Portal-fx&tabs=azure-cli#admin-account

`az containerapp show --name myapi --resource-group myResourceGroup --query "configuration.ingress.fqdn" --output table`

## Tear down

`az containerapp env delete -n myContainerAppEnv -g MyResourceGroup -y`

## Deploy database

# Create Server:

`az postgres server create --name server --resource-group myResourceGroup --location uksouth --admin-user adminuser --admin-password password`

`az postgres db create -g myResourceGroup -s server -n postgres`

# Connect locally

Update connection string in `appsettings.json`

Being sure to specify SSL

`"DefaultConnection": "Host=server.postgres.database.azure.com;Port=5432;Database=postgres;Username=<user@server>;Password=<PASSWORD>;SslMode=Require;"`

Add your local IP to the firewall settings (settings>connection security)

`dotnet ef database update`

`dotnet run`

## Deploy webapp and connect

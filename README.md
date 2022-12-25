# simple-azure-appconfiguration-with-managed-identity

Simple demo that demonstrates Azure App Configuration running with a Managed Identity.

Examples shows the refresh feature.

This repo cloned from Azure Product Team: https://learn.microsoft.com/en-us/azure/azure-app-configuration/quickstart-aspnet-core-app?tabs=core6x

Added instructions for operating both locally and in Azure -- see program.cs

Will need add environment variable to appsettings or user secrets: 

{
  "ConnectionStrings:AppConfig": "<connection string from Azure App Config>",
  "AppConfigEndpoint": "<Endpoint for Azure App Configuraiton>"
}

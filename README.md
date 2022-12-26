# simple-azure-appconfiguration-with-managed-identity

Simple demo that demonstrates Azure App Configuration running with a Managed Identity. Cloned from Azure Product Team: https://learn.microsoft.com/en-us/azure/azure-app-configuration/quickstart-aspnet-core-app?tabs=core6x

Examples includes the refresh feature.

Added instructions (comments in program.cs) for operating both locally and in Azure.

Will need to add following environment variable to local user secrets, prefereably, or appsettings.json: 

{
  "ConnectionStrings:AppConfig": "<connection string from Azure App Config>",
  "AppConfigEndpoint": "<Endpoint for Azure App Configuraiton>"
}

Added Azure Key Vault integration. Follow these instructions: https://learn.microsoft.com/en-us/azure/azure-app-configuration/use-key-vault-references-dotnet-core?tabs=core5x

When naming keys, remember Key Vault requires a '--' to separate name parts, while App Config needs ':'. Ex: “AppSettings:Db:Password” for the App Config connection and “AppSettings–Db–Password” for Key Vault

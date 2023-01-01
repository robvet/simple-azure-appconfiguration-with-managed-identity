using System;
using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Azure.AppConfiguration.WebDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // This example uses the Microsoft.Azure.AppConfiguration.AspNetCore NuGet package:
        // - Establishes the connection to Azure App Configuration using DefaultAzureCredential.
        // - Loads configuration from Azure App Configuration.
        // - Sets up dynamic configuration refresh triggered by a sentinel key.

        // Link to demo:
        // https://learn.microsoft.com/en-us/azure/azure-app-configuration/quickstart-aspnet-core-app?tabs=core6x

        // Prerequisite
        // - An Azure App Configuration store is created
        // - The application identity is granted "App Configuration Data Reader" role in the App Configuration store
        // - "AzureAppConfigurationEndpoint" is set to the App Configuration endpoint in either appsettings.json or environment
        // - The "WebDemo" section in the appsettings.json is imported to the App Configuration store
        // - A sentinel key "WebDemo:Sentinel" is created in App Configuration to signal the refresh of configuration
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration(builder =>
                    {

                        var settings = builder.Build();
                        string appConfigurationEndpoint = settings["AppConfigEndpoint"];
                        if (!string.IsNullOrEmpty(appConfigurationEndpoint))
                        {
                            builder.AddAzureAppConfiguration(options =>
                            {
                                // This code runs locally and in Azure, but...
                                // 1) Must have Managed Identity for App Host that's configured with App Configuration
                                // 2) Must grant local account that runs in VS or VS Code to...
                                //    a) App Configuration Data Owner
                                //    b) App Configuraiton Data Reader
                                //    from the IAM tab in App Configuration.
                                //    Note that access to both roles are required
                                //    Link: https://stackoverflow.com/questions/66585947/local-development-access-to-azure-app-configuration-with-a-managed-identity

                                options.Connect(new Uri(appConfigurationEndpoint), new DefaultAzureCredential())
                                       .Select(keyFilter: "WebDemo:*")
                                       .ConfigureRefresh((refreshOptions) =>
                                       {
                                           // Indicates that all configuration should be refreshed when the given key has changed.
                                           refreshOptions.Register(key: "WebDemo:Sentinel", refreshAll: true);
                                       })
                                       .ConfigureKeyVault(kv =>
                                       {
                                            kv.SetCredential(new DefaultAzureCredential());
                                       });
                            });
                        }
                    }


                        // This code works for Managed Identity only. Will not run locally. Also, no refresh
                        //var settings = builder.Build();
                        //builder.AddAzureAppConfiguration(options =>
                        //    options.Connect(new Uri(settings["AppConfigEndpoint"]), new ManagedIdentityCredential()));


                        //// This code works, but uses connection string -- doesn't leverage AAD or managed identity
                        //IConfiguration settings = builder.Build();
                        //string connectionString = settings.GetConnectionString("AppConfig");
                        //// Load configuration from Azure App Configuration
                        //builder.AddAzureAppConfiguration(options =>
                        //{
                        //    options.Connect(connectionString)
                        //           // Load all keys that start with `TestApp:` and have no label
                        //           .Select("WebDemo:*", LabelFilter.Null)
                        //           // Configure to reload entire configuration (all keys for WebDemo if the registered sentinel key is modified
                        //           .ConfigureRefresh(refreshOptions =>
                        //                refreshOptions.Register("WebDemo:Settings:Sentinel", refreshAll: true));
                        //});


                        // Load configuration from Azure App Configuration
                        //builder.AddAzureAppConfiguration(connectionString);

                    );

                    webBuilder.UseStartup<Startup>();
                });
    }
}

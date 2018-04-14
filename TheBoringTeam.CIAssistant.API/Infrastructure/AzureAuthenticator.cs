using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TheBoringTeam.CIAssistant.API.Infrastructure
{
    public class AzureAuthenticator
    {
        public static IAzure GetAzure()
        {
            var filePath = Directory.GetCurrentDirectory() + "\\azureauth.properties";
            var credentials = SdkContext.AzureCredentialsFactory.FromFile(filePath);
            return Azure.Configure().WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic).Authenticate(credentials).WithSubscription("283bb37a-bc98-40ea-b47c-19423aa0fdf5");
        }
    }
}

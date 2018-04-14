using Microsoft.Azure.Management.ResourceManager.Fluent;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TheBoringTeam.CIAssistant.BusinessLogic.Interfaces
{
    public interface IAzureBusinessLogic
    {
        List<IResourceGroup> GetResourceGroups();
        void DeployApplicationAsync(string resourceGroup, string applicationName);
        void GetApplications();
    }
}

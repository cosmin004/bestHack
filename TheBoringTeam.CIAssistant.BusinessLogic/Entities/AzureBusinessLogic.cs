using Microsoft.Azure.Management.AppService.Fluent.Models;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoringTeam.CIAssistant.BusinessLogic.Interfaces;

namespace TheBoringTeam.CIAssistant.BusinessLogic.Entities
{
    public class AzureBusinessLogic: IAzureBusinessLogic
    {
        private IAzure _azure;

        public AzureBusinessLogic(IAzure azure)
        {
            this._azure = azure;
        }

        public List<IResourceGroup> GetResourceGroups()
        {
            return _azure.ResourceGroups.List().ToList();
        }

        public void DeployApplicationAsync(string resourceGroup, string applicationName)
        {
            _azure.WebApps.Inner.SwapSlotWithProductionWithHttpMessagesAsync(resourceGroup, applicationName, new CsmSlotEntityInner("dev", false));
        }
        
        public void GetApplications()
        {
            //var x = _azure.WebApps.List().ToList();
            //var y = _azure.WebApps.Inner.CreateOrUpdateWithHttpMessagesAsync("ciassistant", "newTestApplication", new SiteInner());
            //var z = _azure.WebApps.GetByResourceGroup("ciassistant","really-really-awesome-app");
            //var csm = _azure.ActivityLogs.ListEventCategories.;
            var disks = _azure.Disks.List().ToList();
            var dbs = _azure.StorageAccounts.List().ToList();
            var ds = _azure.ContainerServices.List().ToList();
            var sql = _azure.SqlServers.List().ToList();
            var dss = _azure.Deployments.List().ToList();
            var qqq = _azure.AppServices.AppServicePlans.List().ToList();
            //var aaa = _azure.AppServices.Inner.WebApps.BeginCreateOrUpdateWithHttpMessagesAsync("")
        }
    }
}

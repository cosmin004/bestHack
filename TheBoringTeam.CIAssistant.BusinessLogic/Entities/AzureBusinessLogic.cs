using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.AppService.Fluent.Models;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
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

        public void DeployApplication(string resourceGroup, string applicationName)
        {
            _azure.WebApps.Inner.SwapSlotWithProductionWithHttpMessagesAsync(resourceGroup, applicationName, new CsmSlotEntityInner("dev", false));
        }
        
        public List<IWebApp> GetApplications()
        {
            return _azure.WebApps.List().ToList();
        }

        public IWebApp GetApplication(string resourceGroup, string applicationName)
        {
            return _azure.WebApps.GetByResourceGroup(resourceGroup, applicationName);
        }

        public List<IDeployment> GetDeployments()
        {
            return _azure.Deployments.List().ToList();
        }

        public List<IAppServicePlan> GetAppServicePlans()
        {
            return _azure.AppServices.AppServicePlans.List().ToList();
        }

        public void CreateApp(string name, string resourceGroup)
        {
            _azure.WebApps.Define("newtestapplication").WithRegion(Region.EuropeNorth).WithExistingResourceGroup("ciassistant").WithNewWindowsPlan(PricingTier.StandardS1).Create();
        }

        public void CreateResourceGroup(string name)
        {
            _azure.ResourceGroups.Define(name).WithRegion(Region.EuropeNorth).Create();
        }
    }
}

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
    public class AzureBusinessLogic : IAzureBusinessLogic
    {
        private IAzure _azure;

        public AzureBusinessLogic(IAzure azure)
        {
            this._azure = azure;
        }

        public bool DeployApplication(string resourceGroup, string applicationName, string sourceEnvironment)
        {
            var app = _azure.WebApps.GetByResourceGroup(resourceGroup, applicationName);
            if (app != null)
            {
                _azure.WebApps.Inner
                    .SwapSlotWithProductionWithHttpMessagesAsync(resourceGroup, applicationName, new CsmSlotEntityInner(sourceEnvironment, false));
                return true;
            }
            return false;
        }

        public List<IWebApp> GetApplications()
        {
            return _azure.WebApps.List().ToList();
        }

        public IWebApp GetApplication(string resourceGroup, string applicationName)
        {
            return _azure.WebApps.GetByResourceGroup(resourceGroup, applicationName);
        }

        public void CreateApp(string name, string resourceGroup)
        {
            _azure.WebApps.Define("newtestapplication")
                .WithRegion(Region.EuropeNorth)
                .WithExistingResourceGroup(resourceGroup)
                .WithNewWindowsPlan(PricingTier.StandardS1)
                .Create();
        }

        public List<IDeployment> GetDeployments()
        {
            return _azure.Deployments.List().ToList();
        }

        public List<IAppServicePlan> GetAppServicePlans()
        {
            return _azure.AppServices.AppServicePlans.List().ToList();
        }

        public void CreateAppServicePlan(string name, string resourceGroup)
        {
            _azure.AppServices.AppServicePlans.Define(name)
                .WithRegion(Region.EuropeNorth)
                .WithExistingResourceGroup(resourceGroup)
                .WithPricingTier(PricingTier.StandardS1)
                .WithOperatingSystem(Microsoft.Azure.Management.AppService.Fluent.OperatingSystem.Windows)
                .Create();
        }

        public void CreateAppWithDeployment(string name, string resourceGroup, string repository, string branch)
        {
            var app = _azure.WebApps.Define(name)
                .WithRegion(Region.EuropeNorth)
                .WithExistingResourceGroup(resourceGroup)
                .WithNewWindowsPlan(PricingTier.StandardS2)
                .Create();

            app.DeploymentSlots.Define("dev")
                .WithBrandNewConfiguration()
                .DefineSourceControl()
                .WithPublicGitRepository(repository)
                .WithBranch(branch)
                .Attach()
                .Create();
        }

        public void CreateResourceGroup(string name)
        {
            _azure.ResourceGroups.Define(name).WithRegion(Region.EuropeNorth).Create();
        }

        public List<IResourceGroup> GetResourceGroups()
        {
            return _azure.ResourceGroups.List().ToList();
        }
    }
}

﻿using Microsoft.Azure.Management.AppService.Fluent;
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
        void DeployApplication(string resourceGroup, string applicationName);
        List<IWebApp> GetApplications();
        IWebApp GetApplication(string resourceGroup, string applicationName);
        List<IDeployment> GetDeployments();
        List<IAppServicePlan> GetAppServicePlans();
        void CreateApp(string name, string resourceGroup);
        void CreateResourceGroup(string name);
    }
}

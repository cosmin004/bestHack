﻿using ApiAiSDK.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using TheBoringTeam.CIAssistant.BusinessEntities.Enums;
using TheBoringTeam.CIAssistant.BusinessLogic.Interfaces;

namespace TheBoringTeam.CIAssistant.BusinessLogic.Entities
{
    public class ActionBusinessLogic : IActionBusinessLogic
    {
        private readonly IAzureBusinessLogic _azureBusinessLogic;
        private readonly IBaseBusinessLogic<BusinessEntities.Entities.Action> _actionBusinessLogic;

        public ActionBusinessLogic(IAzureBusinessLogic azureBusinessLogic, IBaseBusinessLogic<BusinessEntities.Entities.Action> actionBusinessLogic)
        {
            this._azureBusinessLogic = azureBusinessLogic;
            this._actionBusinessLogic = actionBusinessLogic;
        }

        public string HandleAction(AIResponse response, RolesEnum userRole)
        {
            ActionsEnum action;
            Enum.TryParse<ActionsEnum>(response.Result.Action, out action);

            if (response.Result.ActionIncomplete)
            {
                return response.Result.Fulfillment.Speech;
            }

            switch (action)
            {
                case ActionsEnum.Default:
                    return response.Result.Fulfillment.Speech;
                case ActionsEnum.CreateApplicationAction:
                    var newAppName = response.Result.Contexts.FirstOrDefault()?
                        .Parameters["name"]?.ToString();

                    if (newAppName == null)
                        return "The name can't be empty";

                    var resourceGroup = response.Result.Contexts.FirstOrDefault()?
                        .Parameters["resourceGroup"]?.ToString();

                    if (resourceGroup == null)
                        return "The resourceGroup can't be empty";

                    var repository = response.Result.Contexts.FirstOrDefault()?
                        .Parameters["repository"]?.ToString();

                    if (repository == null)
                        return "The repository can't be empty";

                    var branch = response.Result.Contexts.FirstOrDefault()?
                        .Parameters["branch"]?.ToString();

                    if (branch == null)
                        return "The branch can't be empty";

                    var hasDev = response.Result.Contexts.FirstOrDefault()?
                        .Parameters["hasDev"]?.ToString();

                    if (branch == null)
                        return "You should say if you want development slot or not";

                    this._azureBusinessLogic.CreateAppWithDeployment(newAppName, resourceGroup, repository, branch, hasDev == "yes");
                    this._actionBusinessLogic.Insert(new BusinessEntities.Entities.Action() { ActionType = ActionsEnum.CreateApplicationAction, Description = newAppName + " app was created" });
                    return "Application has been created successfully. Enjoy!";

                case ActionsEnum.CreateResourceGroup:
                    var name = response.Result.Contexts.FirstOrDefault()?
                        .Parameters["name"]?.ToString();

                    if (name == null)
                        return "The name can't be empty";

                    this._azureBusinessLogic.CreateResourceGroup(name);
                    this._actionBusinessLogic.Insert(new BusinessEntities.Entities.Action() { ActionType = ActionsEnum.CreateResourceGroup, Description = name + " resource group was created" });
                    return response.Result.Fulfillment.Speech;
                case ActionsEnum.ShowServicePlans:
                    var servicePlans = this._azureBusinessLogic.GetAppServicePlans();

                    return "This is what I've found: " + String.Join(',', servicePlans.Select(f => f.Name));
                case ActionsEnum.ShowDeployments:
                    var deployments = this._azureBusinessLogic.GetDeployments();

                    return "This is what I've found: " + String.Join(',', deployments.Select(f => f.Name));
                case ActionsEnum.ShowResourceGroups:
                    var rsGroups = this._azureBusinessLogic.GetResourceGroups();

                    return "I've found the following resource groups: " + String.Join(',', rsGroups.Select(f => f.Name));
                case ActionsEnum.ShowApplications:
                    var apps = this._azureBusinessLogic.GetApplications();

                    return "I've found the following apps: " + String.Join(',', apps.Select(f => f.Name));
                case ActionsEnum.ShowApplication:
                    var appName = response.Result.Parameters["application"]?.ToString();
                    var rsGroup = response.Result.Parameters["resourceGroup"]?.ToString();

                    if (appName == null)
                        return "I couldn't find the application";

                    if (rsGroup == null)
                        return "I couldn't find the resource group";

                    var application = this._azureBusinessLogic.GetApplication(rsGroup, appName);

                    return "The application you are searching for is hosted on " + application.DefaultHostName + ". It was last modified on " + application.LastModifiedTime + ".";

                case ActionsEnum.DeployWebApp:
                    var applicationName = response.Result.Contexts.FirstOrDefault()?
                        .Parameters["application"]?.ToString();

                    var resoureceGroup = response.Result.Contexts.FirstOrDefault()?
                        .Parameters["resourceGroup"]?.ToString();

                    var fromEnv = response.Result.Contexts.FirstOrDefault()?
                        .Parameters["fromEnv"]?.ToString();

                    if (userRole < RolesEnum.Developer)
                    {
                        return "I am afraid that you are not allowed to do a deployment";
                    }

                    if (applicationName == null)
                        return "I couldn't find the application";

                    if (resoureceGroup == null)
                        return "I couldn't find the resource group";

                    if (!this._azureBusinessLogic.DeployApplication(resoureceGroup, applicationName, fromEnv)) {
                        return "I was unable to start deployment for " + applicationName + " from " + resoureceGroup;
                    }
                    this._actionBusinessLogic.Insert(new BusinessEntities.Entities.Action() { ActionType = ActionsEnum.DeployWebApp, Description = applicationName + " was deployed" });
                    return response.Result.Fulfillment.Speech;
                default:
                    return response.Result.Fulfillment.Speech;
            }
        }
    }
}

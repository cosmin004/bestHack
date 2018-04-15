using ApiAiSDK.Model;
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

        public ActionBusinessLogic(IAzureBusinessLogic azureBusinessLogic)
        {
            this._azureBusinessLogic = azureBusinessLogic;
        }

        public string HandleAction(AIResponse response, RolesEnum userRole)
        {
            ActionsEnum action;
            Enum.TryParse<ActionsEnum>(response.Result.Action, out action);

            switch (action)
            {
                case ActionsEnum.Default:
                    return response.Result.Fulfillment.Speech;
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
                    return response.Result.Fulfillment.Speech;
                default:
                    return response.Result.Fulfillment.Speech;
            }
        }
    }
}

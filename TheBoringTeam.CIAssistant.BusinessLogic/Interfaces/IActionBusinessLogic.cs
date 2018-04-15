using ApiAiSDK.Model;
using System;
using System.Collections.Generic;
using System.Text;
using TheBoringTeam.CIAssistant.BusinessEntities.Enums;

namespace TheBoringTeam.CIAssistant.BusinessLogic.Interfaces
{
    public interface IActionBusinessLogic
    {
        string HandleAction(AIResponse response, RolesEnum userRole);
    }
}

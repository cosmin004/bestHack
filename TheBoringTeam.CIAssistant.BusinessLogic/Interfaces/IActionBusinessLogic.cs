using ApiAiSDK.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheBoringTeam.CIAssistant.BusinessLogic.Interfaces
{
    public interface IActionBusinessLogic
    {
        string HandleAction(AIResponse response);
    }
}

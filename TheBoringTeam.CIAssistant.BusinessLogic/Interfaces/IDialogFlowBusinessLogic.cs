using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheBoringTeam.CIAssistant.BusinessEntities.Entities;
using TheBoringTeam.CIAssistant.BusinessEntities.Enums;

namespace TheBoringTeam.CIAssistant.BusinessLogic.Interfaces
{
    public interface IDialogFlowBusinessLogic
    {
        Task<DialogFlowResponse> Talk(string sentence, string sessionId, RolesEnum userRole);
    }
}

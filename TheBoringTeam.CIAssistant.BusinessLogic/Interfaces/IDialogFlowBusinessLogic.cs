using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheBoringTeam.CIAssistant.BusinessEntities.Entities;

namespace TheBoringTeam.CIAssistant.BusinessLogic.Interfaces
{
    public interface IDialogFlowBusinessLogic
    {
        Task<DialogFlowResponse> Talk(string sentence, string sessionId);
    }
}

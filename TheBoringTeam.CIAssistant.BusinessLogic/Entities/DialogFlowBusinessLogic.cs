using ApiAiSDK;
using ApiAiSDK.Model;
using ApiAiSDK.NETCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheBoringTeam.CIAssistant.BusinessEntities.Entities;
using TheBoringTeam.CIAssistant.BusinessEntities.Enums;
using TheBoringTeam.CIAssistant.BusinessLogic.Interfaces;

namespace TheBoringTeam.CIAssistant.BusinessLogic.Entities
{
    public class DialogFlowBusinessLogic : IDialogFlowBusinessLogic
    {
        private readonly IConfiguration _configuration;
        private readonly IActionBusinessLogic _actionBusinessLogic;

        public DialogFlowBusinessLogic(IConfiguration configuration, IActionBusinessLogic actionBusinessLogic)
        {
            this._configuration = configuration;
            this._actionBusinessLogic = actionBusinessLogic;
        }

        public async Task<DialogFlowResponse> Talk(string sentence, string sessionId, RolesEnum userRole)
        {
            var config = new AIConfiguration(this._configuration["dialogFlowToken"], 
                SupportedLanguage.English);
            if (sessionId != null) config.SessionId = sessionId;
            ApiAi apiAi = new ApiAi(config);

            AIResponse result = await apiAi.TextRequestAsync(sentence);

            string responseText = this._actionBusinessLogic.HandleAction(result, userRole);

            DialogFlowResponse response = new DialogFlowResponse()
            {
                Sentence = responseText,
                Action = result.Result.Action,
                SessionId = result.SessionId
            };

            return response;
        }
    }
}

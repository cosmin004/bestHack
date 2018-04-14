using ApiAiSDK;
using ApiAiSDK.Model;
using ApiAiSDK.NETCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheBoringTeam.CIAssistant.BusinessEntities.Entities;
using TheBoringTeam.CIAssistant.BusinessLogic.Interfaces;

namespace TheBoringTeam.CIAssistant.BusinessLogic.Entities
{
    public class DialogFlowBusinessLogic : IDialogFlowBusinessLogic
    {
        private readonly IConfiguration _configuration;

        public DialogFlowBusinessLogic(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task<DialogFlowResponse> Talk(string sentence, string sessionId)
        {
            var config = new AIConfiguration(this._configuration["dialogFlowToken"], 
                SupportedLanguage.English);
            if (sessionId != null) config.SessionId = sessionId;
            ApiAi apiAi = new ApiAi(config);

            AIResponse result = await apiAi.TextRequestAsync(sentence);

            DialogFlowResponse response = new DialogFlowResponse()
            {
                Sentence = result.Result.Fulfillment.Speech,
                Action = result.Result.Action,
                SessionId = result.SessionId
            };

            return response;
        }
    }
}

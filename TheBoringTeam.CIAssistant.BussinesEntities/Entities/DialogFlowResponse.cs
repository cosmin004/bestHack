using System;
using System.Collections.Generic;
using System.Text;

namespace TheBoringTeam.CIAssistant.BusinessEntities.Entities
{
    public class DialogFlowResponse
    {
        public string Sentence { get; set; }
        public string SessionId { get; set; }
        public string Action { get; set; }
    }
}

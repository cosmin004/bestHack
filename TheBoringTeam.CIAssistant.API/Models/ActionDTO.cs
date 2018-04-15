using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBoringTeam.CIAssistant.BusinessEntities.Enums;

namespace TheBoringTeam.CIAssistant.API.Models
{
    public class ActionDTO
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public ActionsEnum ActionType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBoringTeam.CIAssistant.BusinessLogic.Interfaces;
using TheBoringTeam.CIAssistant.API.Models;
using AutoMapper;
using TheBoringTeam.CIAssistant.BusinessEntities.Entities;
using TheBoringTeam.CIAssistant.API.Infrastructure;
using TheBoringTeam.CIAssistant.BusinessEntities.Enums;

namespace TheBoringTeam.CIAssistant.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/assistant")]
    public class AssistantController : Controller
    {
        private readonly IAzureBusinessLogic _azureBL;
        private readonly IDialogFlowBusinessLogic _dialogFlowBusinessLogic;
        private readonly IMapper _mapper;
        private readonly IBaseBusinessLogic<BusinessEntities.Entities.Action> _actionBusinessLogic;

        public AssistantController(IAzureBusinessLogic azureBL, 
            IDialogFlowBusinessLogic dialogFlowBusinessLogic,
            IMapper mapper,
            IBaseBusinessLogic<BusinessEntities.Entities.Action> actionBusinessLogic)
        {
            this._azureBL = azureBL;
            this._dialogFlowBusinessLogic = dialogFlowBusinessLogic;
            this._mapper = mapper;
            this._actionBusinessLogic = actionBusinessLogic;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GET()
        {
            //_azureBL.CreateAppServicePlan("newTestServicePlanz", "ciassistant");
            //_azureBL.DeployApplication("ciassistant", "really-really-awesome-app");
            return Ok();
        }

        [HttpGet]
        [Route("actions")]
        public IActionResult ACTIONS()
        {
            var actions = _actionBusinessLogic.Search(f => true).ToList();
            //return Ok(_mapper.Map<ActionDTO>(actions));
            return Ok(actions);
        }

        [HttpPost]
        [BearerAuthentication]
        [RoleFilter(RolesEnum.Viewer)]
        [Route("")]
        public async Task<IActionResult> POST([FromBody] SentenceDTO sentence)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DialogFlowResponse result = await this._dialogFlowBusinessLogic
                .Talk(sentence.Sentence, sentence.SessionId, ((RolesEnum)int.Parse(User.Claims.FirstOrDefault(f => f.Type == "Role").Value)));

            return Ok(_mapper.Map<SentenceDTO>(result));
        }
    }
}
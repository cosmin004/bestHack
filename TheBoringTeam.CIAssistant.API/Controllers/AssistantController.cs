using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBoringTeam.CIAssistant.BusinessLogic.Interfaces;

namespace TheBoringTeam.CIAssistant.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/assistant")]
    public class AssistantController : Controller
    {
        private readonly IAzureBusinessLogic _azureBL;
        public AssistantController(IAzureBusinessLogic azureBL)
        {
            this._azureBL = azureBL;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GET()
        {
            _azureBL.GetApplications();
            //_azureBL.DeployApplication("ciassistant", "really-really-awesome-app");
            return Ok();
        }

        [HttpPost]
        [Route("")]
        public IActionResult POST([FromBody] String message)
        {
            return Ok();
        }
    }
}
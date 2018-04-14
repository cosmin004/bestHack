using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBoringTeam.CIAssistant.BusinessEntities.Entities;
using TheBoringTeam.CIAssistant.BusinessLogic.Interfaces;

namespace TheBoringTeam.CIAssistant.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/users")]
    public class UserController : Controller
    {
        private IBaseBusinessLogic<User> _userBL;

        public UserController(IBaseBusinessLogic<User> userBL)
        {
            this._userBL = userBL;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GET()
        {
            IEnumerable<User> users = _userBL.Search(f => true);
            return Ok(users);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GETById(string id)
        {
            User user = _userBL.GetById(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult PUT(string id, [FromBody] User user)
        {
            User currentUser = _userBL.GetById(id);

            if (currentUser == null)
                return NotFound();

            _userBL.Update(user);

            return Ok();
        }

        [HttpPost]
        [Route("")]
        public IActionResult POST([FromBody] User user)
        {
            if (user == null)
                return BadRequest(user);

            _userBL.Insert(user);
            return Ok(user.Id);
        }
    }
}
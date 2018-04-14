using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBoringTeam.CIAssistant.BusinessEntities.Entities;
using TheBoringTeam.CIAssistant.BusinessLogic.Interfaces;
using AutoMapper;
using TheBoringTeam.CIAssistant.API.Models;
using Microsoft.Azure.Management.Fluent;

namespace TheBoringTeam.CIAssistant.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/users")]
    public class UserController : Controller
    {
        private readonly IBaseBusinessLogic<User> _userBL;
        private readonly IMapper _mapper;
        private readonly IAzure _azure;

        public UserController(IMapper mapper, IBaseBusinessLogic<User> userBL, IAzure azure)
        {
            this._userBL = userBL;
            this._mapper = mapper;
            this._azure = azure;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GET()
        {
            IEnumerable<User> users = _userBL.Search(f => true);
            return Ok(_mapper.Map<List<UserDTO>>(users));
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GETById(string id)
        {
            User user = _userBL.GetById(id);

            if (user == null)
                return NotFound();

            return Ok(_mapper.Map<UserDTO>(user));
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DELETE(string id)
        {
            User currentUser = _userBL.GetById(id);

            if (currentUser == null)
                return NotFound();

            _userBL.Delete(currentUser);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult PUT(string id, [FromBody] UserUpdateDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User currentUser = _userBL.GetById(id);

            if (currentUser == null)
                return NotFound();

            User toUpdate = _mapper.Map<User>(user);
            toUpdate.DateCreation = currentUser.DateCreation;
            toUpdate.Id = currentUser.Id;
            _userBL.Update(toUpdate);

            return Ok();
        }

        [HttpPost]
        [Route("")]
        public IActionResult POST([FromBody] UserCreateDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User toAdd = _mapper.Map<User>(user);

            _userBL.Insert(toAdd);
            return Ok(toAdd.Id);
        }
    }
}
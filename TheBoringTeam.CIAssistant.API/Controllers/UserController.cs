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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using TheBoringTeam.CIAssistant.API.Infrastructure;
using System.Security.Cryptography;
using TheBoringTeam.CIAssistant.BusinessEntities.Enums;

namespace TheBoringTeam.CIAssistant.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/users")]
    public class UserController : Controller
    {
        private readonly IBaseBusinessLogic<User> _userBL;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserController(IMapper mapper, IBaseBusinessLogic<User> userBL, IConfiguration configuration)
        {
            this._userBL = userBL;
            this._mapper = mapper;
            this._configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        [BasicAuthentication]
        public IActionResult LOGIN()
        {
            string id = User.Claims.FirstOrDefault(f => f.Type == "UserId").Value;

            var currentUser = _userBL.GetById(id);

            var claims = new[]
            {
                new Claim("UserId", id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["appSecret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expirationDate = DateTime.UtcNow.AddHours(6);

            var token = new JwtSecurityToken(
                claims: claims,
                issuer: "IdentityServer",
                expires: expirationDate,
                signingCredentials: creds);

            return Ok(new
            {
                user = _mapper.Map<UserDTO>(currentUser),
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = expirationDate
            });
        }

        [HttpGet]
        [Route("me")]
        [BearerAuthentication]
        public IActionResult ME()
        {
            string id = User.Claims.FirstOrDefault(f => f.Type == "UserId").Value;

            var currentUser = _userBL.GetById(id);

            return Ok(_mapper.Map<UserDTO>(currentUser));
        }

        [HttpGet]
        [Route("")]
        [BearerAuthentication]
        [RoleFilter(RolesEnum.Owner)]
        public IActionResult GET()
        {
            IEnumerable<User> users = _userBL.Search(f => true);
            return Ok(_mapper.Map<List<UserDTO>>(users));
        }

        [HttpGet]
        [Route("{id}")]
        [BearerAuthentication]
        [RoleFilter(RolesEnum.Owner)]
        public IActionResult GETById(string id)
        {
            User user = _userBL.GetById(id);

            if (user == null)
                return NotFound();

            return Ok(_mapper.Map<UserDTO>(user));
        }

        [HttpDelete]
        [Route("{id}")]
        [BearerAuthentication]
        [RoleFilter(RolesEnum.Owner)]
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
        [BearerAuthentication]
        [RoleFilter(RolesEnum.Owner)]
        public IActionResult PUT(string id, [FromBody] UserUpdateDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User currentUser = _userBL.GetById(id);

            if (currentUser == null)
                return NotFound();

            if (_userBL.Search(f => f.Username == user.Name && f.Id != id).Any())
            {
                ModelState.AddModelError("Name", "Name is already being used");
                return BadRequest(ModelState);
            }

            User toUpdate = _mapper.Map<User>(user);
            toUpdate.DateCreation = currentUser.DateCreation;
            toUpdate.Id = currentUser.Id;
            toUpdate.Password = currentUser.Password;
            _userBL.Update(toUpdate);

            return Ok();
        }

        [HttpPost]
        [Route("")]
        [BearerAuthentication]
        [RoleFilter(RolesEnum.Owner)]
        public IActionResult POST([FromBody] UserCreateDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(_userBL.Search(f => f.Username == user.Name).Any())
            {
                ModelState.AddModelError("Name", "Name is already being used");
                return BadRequest(ModelState);
            }

            User toAdd = _mapper.Map<User>(user);

            using (var md5 = MD5.Create())
            {
                byte[] array = md5.ComputeHash(Encoding.ASCII.GetBytes(toAdd.Password));
                toAdd.Password = Encoding.ASCII.GetString(array);
            }

            _userBL.Insert(toAdd);
            return Ok(toAdd.Id);
        }
    }
}
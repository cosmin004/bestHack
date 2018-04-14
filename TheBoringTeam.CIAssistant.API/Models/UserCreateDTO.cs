using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheBoringTeam.CIAssistant.API.Models
{
    public class UserCreateDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "You have to provide a name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "You have to provide a username")]
        [MinLength(5, ErrorMessage = "The username must have at least 5 characters")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "You have to provide a password")]
        [MinLength(5, ErrorMessage = "The password must have at least 5 characters")]
        public string Password { get; set; }
    }
}

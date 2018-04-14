using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TheBoringTeam.CIAssistant.BusinessEntities.Enums;

namespace TheBoringTeam.CIAssistant.API.Models
{
    public class UserUpdateDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "You have to provide a name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "You have to provide an email address")]
        [EmailAddress(ErrorMessage = "You have to provide a valid email address")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "You have to provide a valid role")]
        public RolesEnum Role { get; set; }
    }
}

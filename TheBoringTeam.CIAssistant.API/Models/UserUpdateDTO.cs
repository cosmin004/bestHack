using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheBoringTeam.CIAssistant.API.Models
{
    public class UserUpdateDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "You have to provide a name")]
        public string Name { get; set; }
    }
}

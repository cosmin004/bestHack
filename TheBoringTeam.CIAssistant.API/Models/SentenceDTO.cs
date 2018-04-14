using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheBoringTeam.CIAssistant.API.Models
{
    public class SentenceDTO
    {
        [Required(ErrorMessage = "You have to provide a sentence")]
        public string Sentence { get; set; }

        public string SessionId { get; set; }

        public string Action { get; set; }

    }
}

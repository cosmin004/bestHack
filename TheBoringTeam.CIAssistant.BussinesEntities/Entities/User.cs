using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using TheBoringTeam.CIAssistant.BusinessEntities.Interfaces;

namespace TheBoringTeam.CIAssistant.BusinessEntities.Entities
{
    public class User : IIdentifiable, ITrackable
    {
        [BsonId]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime? DateModification { get; set; }
    }
}

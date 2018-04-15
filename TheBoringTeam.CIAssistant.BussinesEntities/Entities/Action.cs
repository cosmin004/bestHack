using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using TheBoringTeam.CIAssistant.BusinessEntities.Interfaces;

namespace TheBoringTeam.CIAssistant.BusinessEntities.Entities
{
    public class Action : IIdentifiable, ITrackable
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Description { get; set; }

        public string ActionType { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime? DateModification { get; set; }
    }
}

﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using TheBoringTeam.CIAssistant.BusinessEntities.Enums;
using TheBoringTeam.CIAssistant.BusinessEntities.Interfaces;

namespace TheBoringTeam.CIAssistant.BusinessEntities.Entities
{
    public class User : IIdentifiable, ITrackable
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        [BsonRepresentation(BsonType.String)]
        public RolesEnum Role { get;set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime? DateModification { get; set; }
    }
}

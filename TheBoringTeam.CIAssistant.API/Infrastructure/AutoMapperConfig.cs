using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBoringTeam.CIAssistant.API.Models;
using TheBoringTeam.CIAssistant.BusinessEntities.Entities;

namespace TheBoringTeam.CIAssistant.API.Infrastructure
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserCreateDTO, User>();
            CreateMap<UserUpdateDTO, User>();
        }
    }
}

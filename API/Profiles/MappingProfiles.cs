using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using AutoMapper;
using Domain.Entities;

namespace API.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<RoleDTO, Roles>().ReverseMap();
            CreateMap<RegisterDTO, User>().ReverseMap();
        }
    }
}
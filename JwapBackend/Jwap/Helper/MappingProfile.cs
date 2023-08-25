using AutoMapper;
using Jwap.API.Dtos;
using Jwap.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwap.API.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Message, MessageDto>().ReverseMap();
            CreateMap<CallOffer, CallOfferDto>().ReverseMap();
            CreateMap<User, ConnectionsDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();

        }
    }
}

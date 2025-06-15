using System;
using AutoMapper;
using server.DTO.Post;
using server.DTO.User;
using server.Models;

namespace server;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<Post, PostDTO>().ReverseMap();
    }
}

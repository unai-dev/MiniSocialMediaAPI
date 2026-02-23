using AutoMapper;
using MiniSocialMediaAPI.DTOs.Group;
using MiniSocialMediaAPI.DTOs.User;
using MiniSocialMediaAPI.Entities;

namespace MiniSocialMediaAPI.Utils
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>();

            CreateMap<Group, GroupDTO>();
            CreateMap<AddGroupDTO, Group>();
        }
    }
}

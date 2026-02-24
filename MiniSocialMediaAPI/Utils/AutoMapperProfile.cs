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
            CreateMap<User, UserDTO>().ForMember(
                dest => dest.GroupIds, opt => opt.MapFrom(src => src.Groups.Select(g => g.Id)));

            CreateMap<Group, GroupDTO>().ForMember(
                dest => dest.UserIds, opt => opt.MapFrom(src => src.Users.Select(u => u.Id)));
            CreateMap<AddGroupDTO, Group>();
        }
    }
}

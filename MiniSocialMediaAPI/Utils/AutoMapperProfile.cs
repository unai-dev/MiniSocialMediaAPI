using AutoMapper;
using MiniSocialMediaAPI.DTOs.Coment;
using MiniSocialMediaAPI.DTOs.Group;
using MiniSocialMediaAPI.DTOs.Like;
using MiniSocialMediaAPI.DTOs.Post;
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
            CreateMap<UpdateUserDTO, User>();

            CreateMap<Group, GroupDTO>().ForMember(
                dest => dest.UserIds, opt => opt.MapFrom(src => src.Users.Select(u => u.Id)));
            CreateMap<AddGroupDTO, Group>();

            CreateMap<Post, PostDTO>();
            CreateMap<AddPostDTO, Post>();

            CreateMap<Like, LikeDTO>();

            CreateMap<Coments, ComentDTO>();
            CreateMap<AddComentDTO, Coments>();
        }
    }
}

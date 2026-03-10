using AutoMapper;
using MiniSocialMediaAPI.DTOs.ChatMessage;
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
        /**
         * Constructor para la configuracion de mappings sobre los diferentes DTOs
         */
        public AutoMapperProfile()
        {
            // USER
            CreateMap<User, UserDTO>().ForMember(
                dest => dest.GroupIds, opt => opt.MapFrom(src => src.Groups.Select(g => g.Id)));
            CreateMap<UpdateUserDTO, User>();

            // Group
            CreateMap<Group, GroupDTO>().ForMember(
                dest => dest.UserIds, opt => opt.MapFrom(src => src.Users.Select(u => u.Id)));
            CreateMap<AddGroupDTO, Group>();

            // Post
            CreateMap<Post, PostDTO>();
            CreateMap<AddPostDTO, Post>();

            // Like
            CreateMap<Like, LikeDTO>();

            // Coments
            CreateMap<Coments, ComentDTO>();
            CreateMap<AddComentDTO, Coments>();


            // Chat & Message
            CreateMap<Chat, ChatDTO>();
            CreateMap<Chat, ChatDetailDTO>();
            CreateMap<AddChatDTO, Chat>();

            CreateMap<Message, MessageDTO>();
        }
    }
}

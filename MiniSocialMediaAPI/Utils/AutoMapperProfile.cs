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
    /// <summary>
    /// Configuración de AutoMapper para mapear entidades a DTOs y viceversa.
    /// Define las reglas de conversión entre objetos de base de datos y objetos de transferencia de datos.
    /// Permite abstraer la estructura interna de las entidades del cliente.
    /// </summary>
    public class AutoMapperProfile: Profile
    {
        /// <summary>
        /// Constructor que configura todos los mapeos entre entidades y DTOs.
        /// Los mapeos se definen en un solo lugar centralizando la lógica de transformación.
        /// </summary>
        public AutoMapperProfile()
        {
            // ========== MAPEOS DE USUARIO ==========
            // Convierte una entidad User a UserDTO
            // MapFrom personaliza el mapeo para convertir la lista de grupos en una lista de IDs
            CreateMap<User, UserDTO>().ForMember(
                dest => dest.GroupIds, opt => opt.MapFrom(src => src.Groups.Select(g => g.Id)));
            // Mapea UpdateUserDTO a User para actualizar datos del usuario
            CreateMap<UpdateUserDTO, User>();

            // ========== MAPEOS DE GRUPO ==========
            // Convierte una entidad Group a GroupDTO
            // Transforma la lista de usuarios en una lista de IDs de usuario
            CreateMap<Group, GroupDTO>().ForMember(
                dest => dest.UserIds, opt => opt.MapFrom(src => src.Users.Select(u => u.Id)));
            // Mapea AddGroupDTO a Group para crear un nuevo grupo
            CreateMap<AddGroupDTO, Group>();

            // ========== MAPEOS DE POST ==========
            // Convierte una entidad Post a PostDTO
            CreateMap<Post, PostDTO>();
            // Mapea AddPostDTO a Post para crear un nuevo post
            CreateMap<AddPostDTO, Post>();

            // ========== MAPEOS DE LIKE ==========
            // Convierte una entidad Like a LikeDTO
            CreateMap<Like, LikeDTO>();

            // ========== MAPEOS DE COMENTARIO ==========
            // Convierte una entidad Coments a ComentDTO
            CreateMap<Coments, ComentDTO>();
            // Mapea AddComentDTO a Coments para crear un nuevo comentario
            CreateMap<AddComentDTO, Coments>();

            // ========== MAPEOS DE CHAT Y MENSAJE ==========
            // Convierte una entidad Chat a ChatDTO
            CreateMap<Chat, ChatDTO>();
            // Convierte una entidad Chat a ChatDetailDTO con todos los detalles
            CreateMap<Chat, ChatDetailDTO>();
 
            // Convierte una entidad Message a MessageDTO
            CreateMap<Message, MessageDTO>();
        }
    }
}
using MiniSocialMediaAPI.DTOs.Coment;
using MiniSocialMediaAPI.DTOs.Like;
using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.DTOs.Post
{
    public class PostDTO
    {
        public Guid Id { get; set; }
        public required string Body { get; set; }
        public required string UserId { get; set; }
        public List<LikeDTO> Likes { get; set; } = [];
        public List<ComentDTO> Coments { get; set; } = [];
    }
}

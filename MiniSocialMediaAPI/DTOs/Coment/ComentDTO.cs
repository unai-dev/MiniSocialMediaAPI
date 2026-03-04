using System.ComponentModel.DataAnnotations;

namespace MiniSocialMediaAPI.DTOs.Coment
{
    public class ComentDTO
    {
        public Guid Id { get; set; }
        public required string Body { get; set; }
        public Guid PostId { get; set; }
        public required string UserId { get; set; }
    }
}

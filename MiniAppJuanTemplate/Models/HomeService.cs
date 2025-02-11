using MiniAppJuanTemplate.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace MiniAppJuanTemplate.Models
{
    public class HomeService:BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string ImageName { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
    }
}

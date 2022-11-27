using Quizle.DB.Common.Enums;
using Quizle.DB.Models;
using System.ComponentModel.DataAnnotations;

namespace Quizle.Web.Models
{
    public class BadgeAddViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Name { get; set; } = null!;
        [Required]
        [StringLength(300, MinimumLength = 20)]
        public string Description { get; set; } = null!;
        
        [Required]
        public IFormFile Image { get; set; } = null!;
        [Required]
        public string Rarity { get; set; } = null!;
        public List<string> Rarities { get; set; } = new List<string>();
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Models
{
    public class BadgeDto
    {       
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [Required]
        [StringLength(300)]
        public string Description { get; set; } = null!;
        [Required]
        public byte[] Image { get; set; } = null!;
        [Required]
        public string Rarity { get; set; } = null!;
        public List<string>? OwnerIds { get; set; }
        public int Price { get; set; }
       
    }
}

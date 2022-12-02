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
        public string Name { get; set; } = null!;        
        public string Description { get; set; } = null!;        
        public byte[] Image { get; set; } = null!;
        public string Rarity { get; set; } = null!;
        public string[]? OwnerIds { get; set; }
        public int Price { get; set; }
       
    }
}

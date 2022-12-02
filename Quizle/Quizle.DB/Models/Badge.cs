using Quizle.DB.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.DB.Models
{
    public class Badge
    {
        public Badge()
        {
            ApplicationUsersBadges = new HashSet<ApplicationUserBadge>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [Required]
        [StringLength(300)]
        public string Description { get; set; } = null!;
        [Required]
        public Rarity Rarity { get; set; }
        [Required]
        public byte[] Image { get; set; } = null!;
        [Required]
        public int Price { get; set; }
        [Required]
        public DateTime AcquisitionDate { get; set; }

        public virtual ICollection<ApplicationUserBadge> ApplicationUsersBadges { get; set; }

    }
}

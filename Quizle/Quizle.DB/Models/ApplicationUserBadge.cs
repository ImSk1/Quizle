using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.DB.Models
{
    public class ApplicationUserBadge
    {
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser? ApplicationUser { get; set; }
        [ForeignKey(nameof(Badge))]
        public int BadgeId { get; set; }
        public Badge? Badge { get; set; }

    }
}

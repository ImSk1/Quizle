using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.DB.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            ApplicationUsersBadges = new HashSet<ApplicationUserBadge>();
        }
        public bool HasAnsweredCurrentQuestion { get; set; }
        public virtual ICollection<ApplicationUserBadge> ApplicationUsersBadges { get; set; }

    }
}

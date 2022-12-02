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
            UserQuestions = new HashSet<UserQuestion>();
        }
        [Required]
        public bool HasAnsweredCurrentQuestion { get; set; }
        [Required]
        public int CurrentQuizPoints { get; set; }
        [Required]
        public int AllTimeQuizPoints { get; set; }
        public virtual ICollection<ApplicationUserBadge> ApplicationUsersBadges { get; set; }
        public virtual ICollection<UserQuestion> UserQuestions { get; set; }

    }
}

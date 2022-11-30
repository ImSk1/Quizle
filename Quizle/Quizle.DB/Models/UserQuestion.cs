using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.DB.Models
{
    public class UserQuestion
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Question { get; set; } = null!;
        [Required]
        [StringLength(150)]
        public string SelectedAnswer { get; set; } = null!;
        [Required]
        [StringLength(150)]
        public string CorrectAnswer { get; set; } = null!;
        
        [Required]
        [StringLength(150)]
        public string Difficulty { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;
        public ApplicationUser? User { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.DB.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }
        [Required]        
        public string Question { get; set; }
        [Required]
        [StringLength(50)]
        public string Category { get; set; }
        [Required]
        [StringLength(50)]
        public string Type { get; set; }
        [Required]
        [StringLength(50)]
        public string Difficulty { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}

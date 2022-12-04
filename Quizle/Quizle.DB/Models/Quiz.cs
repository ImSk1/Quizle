using Quizle.DB.Common.Enums;
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
        public Quiz()
        {
            Answers = new HashSet<Answer>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Question { get; set; } = null!;
        [Required]
        [StringLength(50)]
        public string Category { get; set; } = null!;
        [Required]
        [StringLength(50)]
        public string Type { get; set; } = null!;
        [Required]
        public Difficulty Difficulty { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}

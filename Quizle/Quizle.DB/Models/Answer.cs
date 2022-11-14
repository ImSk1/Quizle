using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.DB.Models
{
    public class Answer
    {
        [Key]
        public int Id   { get; set; }
        [StringLength(150)]
        [Required]
        public string Text { get; set; } = null!;
        [Required]
        public bool IsCorrect { get; set; }
        [ForeignKey(nameof(Quiz))]
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
    }
}

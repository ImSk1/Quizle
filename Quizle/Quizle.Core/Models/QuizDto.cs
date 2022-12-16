using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Models
{
    public class QuizDto
    {
        public QuizDto()
        {
            Answers = new List<AnswerDto>();
        }
        [Required]
        public string Question { get; set; } = null!;
        [Required]
        [StringLength(50)]

        public string Category { get; set; } = null!;
        [Required]
        [StringLength(50)]
        public string Type { get; set; } = null!;
        [Required]

        public string Difficulty { get; set; } = null!;
        public List<AnswerDto> Answers { get; set; }
    }
}

using System;
using System.Collections.Generic;
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
        public string Question { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Difficulty { get; set; } = null!;
        public List<AnswerDto> Answers { get; set; }
    }
}

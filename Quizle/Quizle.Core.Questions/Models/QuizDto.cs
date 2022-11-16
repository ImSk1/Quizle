using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Models
{
    public class QuizDto
    {
        public string Question { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Difficulty { get; set; }
        public List<AnswerDto> Answers { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Models
{
    public class UserQuestionDto
    {
        public int Id { get; set; } 
        public string Question { get; set; } = null!;
        public string SelectedAnswer { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Difficulty { get; set; } = null!;
        public string CorrectAnswer { get; set; } = null!;

    }
}

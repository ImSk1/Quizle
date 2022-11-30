using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Models
{
    public  class ProfileDto
    {
        
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool CurrentQuestionStatus { get; set; }
        public int AnsweredQuestionsCount { get; set; }
        public decimal WinratePercent { get; set; }
        public BadgeDto Badge { get; set; }
        public List<UserQuestionDto> AnsweredQuestions { get; set; }
    }
}

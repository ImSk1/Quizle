﻿namespace Quizle.Web.Models
{
    public class ProfileViewModel
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string CurrentQuestionStatus { get; set; } = null!;
        public int AnsweredQuestionsCount { get; set; }
        public decimal WinratePercent { get; set; }
        public BadgeViewModel Badge { get; set; }
        public List<UserQuestionViewModel> AnsweredQuestions { get; set; }
    }
}
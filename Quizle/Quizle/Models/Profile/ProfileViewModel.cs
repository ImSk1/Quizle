namespace Quizle.Web.Models.Profile
{
    public class ProfileViewModel
    {
        public ProfileViewModel()
        {
            AnsweredQuestions = new List<UserQuestionViewModel>();
        }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string CurrentQuestionStatus { get; set; } = null!;
        public int AnsweredQuestionsCount { get; set; }
        public decimal WinratePercent { get; set; }
        public UserBadgeViewModel? UserBadge { get; set; }
        public List<UserQuestionViewModel> AnsweredQuestions { get; set; }
    }
}

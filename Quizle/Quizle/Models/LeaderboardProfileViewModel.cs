namespace Quizle.Web.Models
{
    public class LeaderboardProfileViewModel
    {
        public string ProfileId { get; set; } = null!;
        public string Username { get; set; } = null!;
        public int QuizPoints { get; set; }
    }
}

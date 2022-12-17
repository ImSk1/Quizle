namespace Quizle.Web.Models.Profile
{
    public class LeaderboardProfileViewModel
    {
        public string ProfileId { get; set; } = null!;
        public string Username { get; set; } = null!;
        public int QuizPoints { get; set; }
    }
}

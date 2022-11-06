namespace Quizle.Models
{
    public class QuizViewModel
    {
        public string Question { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Difficulty { get; set; }
        public string CorrectAnswer { get; set; }
        public List<string> Answers { get; set; }
        public string SelectedAnswer { get; set; }

    }
}

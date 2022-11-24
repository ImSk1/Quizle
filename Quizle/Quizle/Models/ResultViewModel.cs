
namespace Quizle.Web.Models
{
    public class ResultViewModel
    {
        public bool IsResultCorrect { get; set; }
        public string CorrectAnswer { get; set; } = null!;
        public string SelectedAnswer { get; set; } = null!;
    }
}

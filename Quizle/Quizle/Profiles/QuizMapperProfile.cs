using AutoMapper;
using Quizle.Core.Questions.Models;
using Quizle.Models;

namespace Quizle.Profiles
{
    public class QuizMapperProfile : Profile
    {
        public QuizMapperProfile()
        {
            CreateMap<QuizDto, QuizViewModel>();
        }
    }
}

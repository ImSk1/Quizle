using AutoMapper;
using Quizle.Core.Questions.Models;
using Quizle.Web.Models;

namespace Quizle.Web.MapperProfiles
{
    public class QuizMapperProfile : Profile
    {
        public QuizMapperProfile()
        {
            CreateMap<QuizDto, QuizViewModel>();
        }
    }
}

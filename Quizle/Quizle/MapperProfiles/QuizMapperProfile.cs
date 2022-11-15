using AutoMapper;
using Quizle.Core.Questions.Models;
using Quizle.DB.Models;
using Quizle.Web.Models;

namespace Quizle.Web.MapperProfiles
{
    public class QuizMapperProfile : Profile
    {
        public QuizMapperProfile()
        {
            CreateMap<QuizDto, QuizViewModel>();
            CreateMap<Quiz, QuizDto>();


            CreateMap<Answer, AnswerDto>()
                .ForMember(a => a.Answer, b => b.MapFrom(src => src.Text));
        }
    }
}

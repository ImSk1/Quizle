using AutoMapper;
using Quizle.Core.Models;
using Quizle.DB.Common.Enums;
using Quizle.DB.Models;
using Quizle.Web.Models;

namespace Quizle.Web.MapperProfiles
{
    public class QuizMapperProfile : Profile
    {
        public QuizMapperProfile()
        {
            CreateMap<QuizDto, QuizViewModel>()
                .ForMember(a => a.Difficulty, b => b.MapFrom(src => (Difficulty)Enum.Parse(typeof(Difficulty), src.Difficulty)));
            
            CreateMap<Quiz, QuizDto>();
            //CreateMap<List<Quiz>, List<QuizDto>>();


            CreateMap<Answer, AnswerDto>()
                .ForMember(a => a.Answer, b => b.MapFrom(src => src.Text));
        }
    }
}

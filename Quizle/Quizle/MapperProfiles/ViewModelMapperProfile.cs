using AutoMapper;
using Quizle.Core.Models;
using Quizle.DB.Common.Enums;
using Quizle.DB.Models;
using Quizle.Web.Models;

namespace Quizle.Web.MapperProfiles
{
    public class ViewModelMapperProfile : Profile
    {
        public ViewModelMapperProfile()
        {
            CreateMap<ProfileDto, ProfileViewModel>()
                .ForMember(a => a.CurrentQuestionStatus, b => b.MapFrom(src => src.CurrentQuestionStatus ? "Has Answered" : "Has Not Answered"));

            CreateMap<UserQuestionDto, UserQuestionViewModel>();
            CreateMap<UserBadgeDto, UserBadgeViewModel>();
            CreateMap<BadgeDto, BadgeViewModel>()
                .ForMember(a => a.Image, b => b.MapFrom(src => string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(src.Image))));
            CreateMap<ProfileDto, LeaderboardProfileViewModel>();
            CreateMap<QuizDto, QuizViewModel>()
                 .ForMember(a => a.Difficulty, b => b.MapFrom(src => (int)Enum.Parse(typeof(Difficulty), src.Difficulty)));            
                
        }

    }
}

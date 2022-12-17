using Quizle.Web.Models.Badge;

namespace Quizle.Web.Models.Profile
{
    public class UserBadgeViewModel
    {
        public BadgeViewModel Badge { get; set; } = null!;
        public string DateAcquired { get; set; } = null!;
    }
}

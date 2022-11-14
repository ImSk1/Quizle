using Quizle.Web.Common;
using System.ComponentModel.DataAnnotations;

namespace Quizle.Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(ValidationConstraints.USER_USERNAME_MAXLENGHT, MinimumLength = ValidationConstraints.USER_USERNAME_MINLENGHT)]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(ValidationConstraints.USER_EMAIL_MAXLENGHT, MinimumLength = ValidationConstraints.USER_EMAIL_MINLENGHT)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(ValidationConstraints.USER_PASSWORD_MAXLENGHT, MinimumLength = ValidationConstraints.USER_PASSWORD_MINLENGHT)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;
    }
}

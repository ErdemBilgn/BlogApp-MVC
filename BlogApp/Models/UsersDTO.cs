using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class UsersDTO
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "{0} field must be between {2} and {1} characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
    }
}
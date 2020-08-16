using System.ComponentModel.DataAnnotations;

namespace assignmentAPI.Model
{
    public class ResetPasswordModel
    {
        public string email { get; set; }
        
        [Required(ErrorMessage = "New password is required")]
        [RegularExpression(@"^([a-zA-Z0-9@*#]{8,15})$", ErrorMessage = "Password must match all alphanumeric character and predefined wild characters. Password must consists of at least 8 characters and not more than 15 characters.")]
        public string newPassword { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Compare(nameof(newPassword), ErrorMessage = "Mismatch with new password")]
        public string confirmPassword { get; set; }

        public string token { get; set; }
    }
}

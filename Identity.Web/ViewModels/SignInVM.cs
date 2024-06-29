using System.ComponentModel.DataAnnotations;

namespace Identity.Web.ViewModels
{
    public class SignInVM
    {
        [EmailAddress(ErrorMessage = "Email format is incorrect.")]
        [Required(ErrorMessage = "Email can't be empty.")]
        [Display(Name = "Email :")]
        public string Email { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password can't be empty.")]
        [Display(Name = "Password :")]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }
}

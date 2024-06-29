using System.ComponentModel.DataAnnotations;

namespace Identity.Web.ViewModels
{
    public class ForgetPasswordVM
    {
        [EmailAddress(ErrorMessage = "Email format is incorrect.")]
        [Required(ErrorMessage = "Email can't be empty.")]
        [Display(Name = "Email :")]
        public string Email { get; set; } = null!;
    }
}

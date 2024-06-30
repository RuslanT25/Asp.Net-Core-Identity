using System.ComponentModel.DataAnnotations;

namespace Identity.Web.ViewModels
{
    public class ResetPasswordVM
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password can't be empty.")]
        [Display(Name = "New password :")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password isn't same.")]
        [Required(ErrorMessage = "Confirm password can't be empty")]
        [Display(Name = "New password again :")]
        public string ConfirmPassword { get; set; } = null!;
    }
}

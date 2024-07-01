using System.ComponentModel.DataAnnotations;

namespace Identity.Web.ViewModels
{
    public class PasswordChangeVM
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password can't be empty.")]
        [Display(Name = "Old password :")]
        public string PasswordOld { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password can't be empty.")]
        [Display(Name = "New password :")]
        public string PasswordNew { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare(nameof(PasswordNew), ErrorMessage = "Password isn't same.")]
        [Required(ErrorMessage = "Confirm password can't be empty")]
        [Display(Name = "New password again :")]
        public string ConfirmPassword { get; set; } = null!;
    }
}

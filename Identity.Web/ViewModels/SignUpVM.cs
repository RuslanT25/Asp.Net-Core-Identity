using System.ComponentModel.DataAnnotations;

namespace Identity.Web.ViewModels
{
    public class SignUpVM
    {
        public SignUpVM()
        {
            
        }

        public SignUpVM(string userName, string email, string password, string phone, string confirmPassword)
        {
            UserName = userName;
            Email = email;
            Password = password;
            Phone = phone;
            ConfirmPassword = confirmPassword;
        }

        [Required(ErrorMessage = "Username can't be empty.")]
        [Display(Name = "Username :")]
        public string UserName { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Email format is incorrect.")]
        [Required(ErrorMessage = "Email can't be empty.")]
        [Display(Name = "Email :")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone can't be empty.")]
        [Display(Name = "Phone :")]
        public string Phone { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password can't be empty.")]
        [Display(Name = "Password :")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password isn't same.")]
        [Required(ErrorMessage = "Confirm password can't be empty")]
        [Display(Name = "Confirm password :")]
        public string ConfirmPassword { get; set; } = null!;
    }
}

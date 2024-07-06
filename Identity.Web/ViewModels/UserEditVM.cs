using Identity.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace Identity.Web.ViewModels
{
    public class UserEditVM
    {
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

        [DataType(DataType.Date)]
        [Display(Name = "BirthDate :")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "City :")]
        public string? City { get; set; }

        [Display(Name = "Profile picture :")]
        public IFormFile? Picture { get; set; }

        [Display(Name = "Gender :")]
        public Gender? Gender { get; set; }

    }
}

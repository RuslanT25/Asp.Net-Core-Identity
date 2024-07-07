using System.ComponentModel.DataAnnotations;

namespace Identity.Web.Areas.Admin.Models
{
    public class RoleAddVM
    {
        [Required(ErrorMessage = "Role name can't be empty.")]
        [Display(Name = "Role name :")]
        public string Name { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;

namespace Identity.Web.Models
{
    public class AppUser : IdentityUser
    {
        public string? City {  get; set; }
        public string? Picture { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }
    }
    public enum Gender : byte
    {
        Man = 1,
        Woman = 2
    }
}

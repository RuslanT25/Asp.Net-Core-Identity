namespace Identity.Web.Services
{
    public interface IEmailService
    {
        Task ResetPassword(string resetPasswordLink, string To);
    }
}

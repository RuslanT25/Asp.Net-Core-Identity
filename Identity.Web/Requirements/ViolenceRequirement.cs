using Microsoft.AspNetCore.Authorization;

namespace Identity.Web.Requirements
{
    public class ViolenceRequirement : IAuthorizationRequirement
    {
        public int Age { get; set; }
    }

    public class ViolenceRequirementHandler : AuthorizationHandler<ViolenceRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViolenceRequirement requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "BirthDate"))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var birthDateClaim = context.User.FindFirst("BirthDate")!;

            var today = DateTime.Now;
            var birthDate = Convert.ToDateTime(birthDateClaim.Value);
            int age = today.Year - birthDate.Year;

            //
            if (birthDate > today.AddYears(-age)) age--;

            if (requirement.Age > age)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}

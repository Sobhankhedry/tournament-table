namespace TournamentProject.Data
{
    using Microsoft.AspNetCore.Identity;

    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = "DuplicateEmail",
                Description = "این ایمیل قبلاً ثبت شده است"
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            // Avoid showing the error or replace it with a generic message
            return new IdentityError
            {
                Code = "DuplicateUserName",
                Description = "" // Empty string to suppress the message
            };
        }
    }

}

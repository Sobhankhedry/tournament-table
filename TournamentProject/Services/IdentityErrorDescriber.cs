using Microsoft.AspNetCore.Identity;

namespace TournamentProject.Services
{

    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = "باشد." + $"{length}" + "رمز عبور شما حداقل باید "
            };
        }


        public override IdentityError DuplicateEmail(string email)
        {
            return base.DuplicateEmail(email);
        }




        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = "رمز عبور شما باید حداقل دارای یک حرف بزرگ انگلیسی باشد"
            };
        }
    }

}

using FluentValidation;
using RestaurantAPI2.Entities;

namespace RestaurantAPI2.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDTO>
    {

        public RegisterUserDtoValidator(RestaurantDbContext dbContext)
        {
            RuleFor(i => i.Email).NotEmpty().EmailAddress();
            RuleFor(i => i.Password).NotEmpty().MinimumLength(6);
            RuleFor(i => i.ConfirmedPassword).Equal(j => j.Password);
            RuleFor(i => i.Email).Custom((value, context) =>
            {
                var duplicatedEmail = dbContext.Users.Any(i => i.Email == value);
                if (duplicatedEmail)
                {
                    context.AddFailure("Email", "Email already in use");
                }
            });
        }
    }
}

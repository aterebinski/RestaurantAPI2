using FluentValidation;

namespace RestaurantAPI2.Models.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        int[] posibblePageSizes = new[] {5,10,15};

        public RestaurantQueryValidator()
        {
            RuleFor(i => i.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(i => i.PageSize).Custom((value, context) =>
            {
                if (!posibblePageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must be in [{string.Join(",", posibblePageSizes)}]");
                };
            });
        }
    }
}

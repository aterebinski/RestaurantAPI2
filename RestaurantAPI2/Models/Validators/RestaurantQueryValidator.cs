using FluentValidation;
using RestaurantAPI2.Entities;

namespace RestaurantAPI2.Models.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        int[] posibblePageSizes = new[] {5,10,15};
        string[] allowedSortByColumnNames = new[]
        {
            nameof(Restaurant.Name),
            nameof(Restaurant.Description),
            nameof(Restaurant.Category)
        };

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
            RuleFor(i => i.OrderBy).Custom((value, context) =>
            {
                if (!allowedSortByColumnNames.Contains(value))
                {
                    context.AddFailure("OrderBy", $"OrderBy must be in [{string.Join(",", allowedSortByColumnNames)}]");
                }
            });
        }
    }
}

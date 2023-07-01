using FluentValidation;

namespace RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Commands.CreateMenuCategory
{
    public class CreateMenuCategoryValidator : AbstractValidator<CreateMenuCategoryCommand>
    {
        public CreateMenuCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(65)
                .Matches("^[a-zA-Z\\- ]+$");

            RuleFor(x => x.Description)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(500)
                .Matches("^[a-zA-Z0-9\\- ,.]+$");
        }
    }
}

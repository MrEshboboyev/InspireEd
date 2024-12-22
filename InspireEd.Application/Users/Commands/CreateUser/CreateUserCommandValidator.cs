using FluentValidation;
using InspireEd.Domain.ValueObjects;

namespace InspireEd.Application.Users.Commands.CreateUser;

internal class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty();

        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(FirstName.MaxLength);

        RuleFor(x => x.LastName).NotEmpty().MaximumLength(LastName.MaxLength);
    }
}
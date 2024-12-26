using FluentValidation;

namespace InspireEd.Application.Faculties.Commands.DepartmentHeads.CreateDepartmentHead;

internal class CreateDepartmentHeadCommandValidator : AbstractValidator<CreateDepartmentHeadCommand>
{
    public CreateDepartmentHeadCommandValidator()
    {
        RuleFor(obj => obj.Email).NotEmpty();
        RuleFor(obj => obj.FirstName).NotEmpty();
        RuleFor(obj => obj.LastName).NotEmpty();
        RuleFor(obj => obj.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters");
    }
}
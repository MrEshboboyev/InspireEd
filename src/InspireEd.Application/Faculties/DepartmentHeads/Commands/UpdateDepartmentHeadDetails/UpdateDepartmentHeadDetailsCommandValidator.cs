using FluentValidation;

namespace InspireEd.Application.Faculties.DepartmentHeads.Commands.UpdateDepartmentHeadDetails;

internal class UpdateDepartmentHeadDetailsCommandValidator : AbstractValidator<UpdateDepartmentHeadDetailsCommand>
{
    public UpdateDepartmentHeadDetailsCommandValidator()
    {
        RuleFor(command => command.DepartmentHeadId)
            .NotEmpty().WithMessage("DepartmentHeadId is required.");

        RuleFor(command => command.FirstName)
            .NotEmpty().WithMessage("First name is required.");

        RuleFor(command => command.LastName)
            .NotEmpty().WithMessage("Last name is required.");

        RuleFor(command => command.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(command => command.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
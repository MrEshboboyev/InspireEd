using FluentValidation;
using InspireEd.Domain.Faculties.ValueObjects;

namespace InspireEd.Application.Faculties.Commands.AddGroupToFaculty;

internal class AddGroupToFacultyCommandValidator : AbstractValidator<AddGroupToFacultyCommand>
{
    public AddGroupToFacultyCommandValidator()
    {
        RuleFor(obj => obj.FacultyId).NotEmpty();
        RuleFor(obj => obj.GroupName)
            .NotEmpty().WithMessage("Please provide a group name")
            .MaximumLength(GroupName.MaxLength).WithMessage("Group name must be 5 characters");
    }
}
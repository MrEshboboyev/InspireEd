using FluentValidation;
using InspireEd.Domain.Faculties.ValueObjects;

namespace InspireEd.Application.Faculties.Groups.Commands.RenameGroup;

internal class RenameGroupCommandValidator : AbstractValidator<RenameGroupCommand>
{
    public RenameGroupCommandValidator()
    {
        RuleFor(obj => obj.FacultyId).NotEmpty();
        
        RuleFor(obj => obj.GroupId).NotEmpty();
        RuleFor(obj => obj.GroupName)
            .NotEmpty().WithMessage("Group name cannot be empty")
            .MaximumLength(GroupName.MaxLength).WithMessage("Group name cannot exceed 5 characters");
    }
}
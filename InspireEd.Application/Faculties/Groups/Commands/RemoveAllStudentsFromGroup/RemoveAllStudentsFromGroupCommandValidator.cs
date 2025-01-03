using FluentValidation;

namespace InspireEd.Application.Faculties.Groups.Commands.RemoveAllStudentsFromGroup;

internal class RemoveAllStudentsFromGroupCommandValidator : AbstractValidator<RemoveAllStudentsFromGroupCommand>
{
    public RemoveAllStudentsFromGroupCommandValidator()
    {
        RuleFor(obj => obj.FacultyId)
            .NotEmpty().WithMessage("FacultyId is required.");
        
        RuleFor(obj => obj.GroupId)
            .NotEmpty().WithMessage("GroupId is required.");
    }
}
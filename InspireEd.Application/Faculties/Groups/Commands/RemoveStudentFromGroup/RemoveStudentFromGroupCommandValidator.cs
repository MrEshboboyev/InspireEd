using FluentValidation;

namespace InspireEd.Application.Faculties.Groups.Commands.RemoveStudentFromGroup;

internal class RemoveStudentFromGroupCommandValidator : AbstractValidator<RemoveStudentFromGroupCommand>
{
    public RemoveStudentFromGroupCommandValidator()
    {
        RuleFor(obj => obj.FacultyId)
            .NotEmpty().WithMessage("FacultyId is required.");
        
        
        RuleFor(obj => obj.GroupId)
            .NotEmpty().WithMessage("GroupId is required.");
        
        
        RuleFor(obj => obj.StudentId)
            .NotEmpty().WithMessage("StudentId is required.");
    }
}
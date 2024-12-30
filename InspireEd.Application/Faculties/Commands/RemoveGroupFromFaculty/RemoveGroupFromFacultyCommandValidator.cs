using FluentValidation;

namespace InspireEd.Application.Faculties.Commands.RemoveGroupFromFaculty;

internal class RemoveGroupFromFacultyCommandValidator : AbstractValidator<RemoveGroupFromFacultyCommand>
{
    public RemoveGroupFromFacultyCommandValidator()
    {
        RuleFor(obj => obj.GroupId).NotEmpty();
        
        RuleFor(obj => obj.FacultyId).NotEmpty();
    }
}
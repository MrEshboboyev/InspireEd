using FluentValidation;

namespace InspireEd.Application.Faculties.Commands.RemoveDepartmentHeadFromFaculty;

internal class RemoveDepartmentHeadFromFacultyCommandValidator : AbstractValidator<RemoveDepartmentHeadFromFacultyCommand>
{
    public RemoveDepartmentHeadFromFacultyCommandValidator()
    {
        RuleFor(obj => obj.FacultyId).NotEmpty();
        
        RuleFor(obj => obj.DepartmentHeadId).NotEmpty();
    }
}
using FluentValidation;

namespace InspireEd.Application.Faculties.Commands.AddDepartmentHeadToFaculty;

internal class AddDepartmentHeadToFacultyCommandValidator : AbstractValidator<AddDepartmentHeadToFacultyCommand>
{
    public AddDepartmentHeadToFacultyCommandValidator()
    {
        RuleFor(obj => obj.FacultyId).NotEmpty();
        
        RuleFor(obj => obj.DepartmentHeadId).NotEmpty();
    }
}
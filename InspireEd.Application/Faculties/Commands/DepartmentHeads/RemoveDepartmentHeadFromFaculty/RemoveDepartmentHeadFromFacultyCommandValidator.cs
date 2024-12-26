using FluentValidation;

namespace InspireEd.Application.Faculties.Commands.DepartmentHeads.RemoveDepartmentHead;

internal class RemoveDepartmentHeadFromFacultyCommandValidator : AbstractValidator<DepartmentHeads.RemoveDepartmentHead.RemoveDepartmentHeadFromFacultyCommand>
{
    public RemoveDepartmentHeadFromFacultyCommandValidator()
    {
        RuleFor(obj => obj.FacultyId).NotEmpty();
        
        RuleFor(obj => obj.DepartmentHeadId).NotEmpty();
    }
}
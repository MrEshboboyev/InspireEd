using FluentValidation;

namespace InspireEd.Application.Faculties.Commands.DepartmentHeads.AddDepartmentHead;

internal class AddDepartmentHeadCommandValidator : AbstractValidator<AddDepartmentHeadCommand>
{
    public AddDepartmentHeadCommandValidator()
    {
        RuleFor(obj => obj.FacultyId).NotEmpty();
        
        RuleFor(obj => obj.DepartmentHeadId).NotEmpty();
    }
}
using FluentValidation;

namespace InspireEd.Application.Faculties.Commands.RemoveDepartmentHeadCommand;

internal class RemoveDepartmentHeadCommandValidator : AbstractValidator<RemoveDepartmentHeadCommand>
{
    public RemoveDepartmentHeadCommandValidator()
    {
        RuleFor(obj => obj.FacultyId).NotEmpty();
        
        RuleFor(obj => obj.DepartmentHeadId).NotEmpty();
    }
}
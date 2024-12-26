using FluentValidation;

namespace InspireEd.Application.Faculties.Commands.DepartmentHeads.DeleteDepartmentHeadCommand;

internal class DeleteDepartmentHeadCommandValidator : AbstractValidator<DeleteDepartmentHeadCommand>
{
    public DeleteDepartmentHeadCommandValidator()
    {
        RuleFor(obj => obj.DepartmentHeadId).NotEmpty();
    }
}
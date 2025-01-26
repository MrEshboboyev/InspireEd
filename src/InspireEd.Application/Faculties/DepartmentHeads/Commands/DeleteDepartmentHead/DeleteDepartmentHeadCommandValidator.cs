using FluentValidation;

namespace InspireEd.Application.Faculties.DepartmentHeads.Commands.DeleteDepartmentHead;

internal class DeleteDepartmentHeadCommandValidator : AbstractValidator<DeleteDepartmentHeadCommand>
{
    public DeleteDepartmentHeadCommandValidator()
    {
        RuleFor(obj => obj.DepartmentHeadId).NotEmpty();
    }
}
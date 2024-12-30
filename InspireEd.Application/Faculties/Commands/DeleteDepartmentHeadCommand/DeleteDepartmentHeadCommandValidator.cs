using FluentValidation;

namespace InspireEd.Application.Faculties.Commands.DeleteDepartmentHeadCommand;

internal class DeleteDepartmentHeadCommandValidator : AbstractValidator<Commands.DeleteDepartmentHeadCommand.DeleteDepartmentHeadCommand>
{
    public DeleteDepartmentHeadCommandValidator()
    {
        RuleFor(obj => obj.DepartmentHeadId).NotEmpty();
    }
}
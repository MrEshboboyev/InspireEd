using FluentValidation;

namespace InspireEd.Application.Faculties.Commands.DeleteFaculty;

internal class DeleteFacultyCommandValidator : AbstractValidator<DeleteFacultyCommand>
{
    public DeleteFacultyCommandValidator()
    {
        RuleFor(obj => obj.FacultyId).NotEmpty();
    }
}
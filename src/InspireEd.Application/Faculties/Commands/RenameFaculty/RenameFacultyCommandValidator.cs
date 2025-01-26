using FluentValidation;
using InspireEd.Domain.Faculties.ValueObjects;

namespace InspireEd.Application.Faculties.Commands.RenameFaculty;

internal class RenameFacultyCommandValidator : AbstractValidator<RenameFacultyCommand>
{
    public RenameFacultyCommandValidator()
    {
        RuleFor(obj => obj.FacultyId).NotEmpty();
        
        RuleFor(obj => obj.FacultyName)
            .NotEmpty().WithMessage("Faculty name is required.")
            .MaximumLength(FacultyName.MaxLength).WithMessage("Faculty name cannot exceed 100 characters.");
    }
}
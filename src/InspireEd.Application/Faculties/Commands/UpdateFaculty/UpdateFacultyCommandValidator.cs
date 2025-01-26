using FluentValidation;
using InspireEd.Domain.Faculties.ValueObjects;

namespace InspireEd.Application.Faculties.Commands.UpdateFaculty;

internal class UpdateFacultyCommandValidator : AbstractValidator<UpdateFacultyCommand>
{
    public UpdateFacultyCommandValidator()
    {
        RuleFor(obj => obj.FacultyId).NotEmpty();
        
        RuleFor(obj => obj.FacultyName)
            .NotEmpty().WithMessage("Faculty name is required.")
            .MaximumLength(FacultyName.MaxLength).WithMessage("Faculty name cannot exceed 100 characters.");
    }
}
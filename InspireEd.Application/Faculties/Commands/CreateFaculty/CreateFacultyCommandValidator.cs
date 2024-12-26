using FluentValidation;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.ValueObjects;

namespace InspireEd.Application.Faculties.Commands.CreateFaculty;

internal class CreateFacultyCommandValidator : AbstractValidator<CreateFacultyCommand>
{
    public CreateFacultyCommandValidator()
    {
        RuleFor(obj => obj.FacultyName)
            .NotEmpty().WithMessage("Faculty name is required.")
            .MaximumLength(FacultyName.MaxLength)
                .WithMessage("Faculty name cannot exceed 100 characters.");
    }
}
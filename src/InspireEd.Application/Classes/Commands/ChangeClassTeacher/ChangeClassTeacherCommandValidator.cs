using FluentValidation;

namespace InspireEd.Application.Classes.Commands.ChangeClassTeacher;

internal class ChangeClassTeacherCommandValidator : AbstractValidator<ChangeClassTeacherCommand>
{
    public ChangeClassTeacherCommandValidator()
    {
        RuleFor(c => c.ClassId).NotEmpty();
        RuleFor(c => c.TeacherId).NotEmpty();
    }
}
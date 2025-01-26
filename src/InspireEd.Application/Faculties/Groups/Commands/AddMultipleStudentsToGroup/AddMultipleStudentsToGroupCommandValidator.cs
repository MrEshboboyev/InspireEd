using FluentValidation;

namespace InspireEd.Application.Faculties.Groups.Commands.AddMultipleStudentsToGroup;

internal class AddMultipleStudentsToGroupCommandValidator : AbstractValidator<AddMultipleStudentsToGroupCommand>
{
    public AddMultipleStudentsToGroupCommandValidator()
    {
        RuleFor(obj => obj.FacultyId)
            .NotEmpty().WithMessage("FacultyId is required.");

        RuleFor(obj => obj.GroupId)
            .NotEmpty().WithMessage("GroupId is required.");

        RuleFor(obj => obj.Students)
            .NotEmpty().WithMessage("Students list is required.")
            .Must(students => students.All(student =>
                !string.IsNullOrWhiteSpace(student.FirstName) &&
                !string.IsNullOrWhiteSpace(student.LastName) &&
                !string.IsNullOrWhiteSpace(student.Email) &&
                !string.IsNullOrWhiteSpace(student.Password)))
            .WithMessage("Each student's first name, last name, email, and password are required.");
    }
}
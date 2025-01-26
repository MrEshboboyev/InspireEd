using FluentValidation;
using InspireEd.Domain.Users.ValueObjects;

namespace InspireEd.Application.Faculties.Groups.Commands.AddStudentToGroup;

internal class AddStudentToGroupCommandValidator : AbstractValidator<AddStudentToGroupCommand>  
{
    public AddStudentToGroupCommandValidator()
    {
        RuleFor(obj => obj.FacultyId).NotEmpty();
        
        RuleFor(obj => obj.GroupId).NotEmpty();
        
        RuleFor(obj => obj.StudentFirstName)
            .NotEmpty().WithMessage("First name cannot be empty")
            .MaximumLength(FirstName.MaxLength)
                .WithMessage($"First name cannot exceed {FirstName.MaxLength} characters");
    
        RuleFor(obj => obj.StudentLastName)
            .NotEmpty().WithMessage("Last name cannot be empty")
            .MaximumLength(LastName.MaxLength).WithMessage($"Last name cannot exceed {LastName.MaxLength} characters");

        RuleFor(obj => obj.StudentEmail)
            .NotEmpty().WithMessage("Email cannot be empty");
        
        RuleFor(obj => obj.StudentPassword)
            .NotEmpty().WithMessage("Password cannot be empty");
    }
}
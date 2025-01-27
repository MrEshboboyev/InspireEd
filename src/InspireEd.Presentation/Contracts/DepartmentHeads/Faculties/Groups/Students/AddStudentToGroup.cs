namespace InspireEd.Presentation.Contracts.DepartmentHeads.Faculties.Groups.Students;

public sealed record AddStudentToGroup(
    string Email,
    string Password,
    string FirstName,
    string LastName);
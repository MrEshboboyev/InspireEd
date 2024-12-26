namespace InspireEd.Presentation.Contracts.Admins;

public sealed record CreateDepartmentHeadRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password);
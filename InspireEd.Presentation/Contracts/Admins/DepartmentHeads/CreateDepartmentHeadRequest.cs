namespace InspireEd.Presentation.Contracts.Admins.DepartmentHeads;

public sealed record CreateDepartmentHeadRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password);
namespace InspireEd.Presentation.Contracts.Admins.DepartmentHeads;

public record UpdateDepartmentHeadDetailsRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password);
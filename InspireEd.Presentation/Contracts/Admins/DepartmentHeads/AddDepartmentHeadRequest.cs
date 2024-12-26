namespace InspireEd.Presentation.Contracts.Admins;

public sealed record AddDepartmentHeadRequest(
    Guid FacultyId,
    Guid DepartmentHeadId);
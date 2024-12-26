namespace InspireEd.Presentation.Contracts.Admins;

public sealed record RemoveDepartmentHeadRequest(
    Guid FacultyId,
    Guid DepartmentHeadId);
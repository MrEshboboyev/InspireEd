namespace InspireEd.Presentation.Contracts.Admins.Faculties;

public sealed record UpdateFacultyRequest(
    Guid FacultyId,
    string FacultyName);
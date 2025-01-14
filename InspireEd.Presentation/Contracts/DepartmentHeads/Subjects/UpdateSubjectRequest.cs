namespace InspireEd.Presentation.Contracts.DepartmentHeads.Subjects;

public sealed record UpdateSubjectRequest(
    string Name,
    string Code,
    int Credit);
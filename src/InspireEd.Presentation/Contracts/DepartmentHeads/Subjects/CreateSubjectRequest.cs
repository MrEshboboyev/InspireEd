namespace InspireEd.Presentation.Contracts.DepartmentHeads.Subjects;

public sealed record CreateSubjectRequest(
    string Name,
    string Code,
    int Credit);
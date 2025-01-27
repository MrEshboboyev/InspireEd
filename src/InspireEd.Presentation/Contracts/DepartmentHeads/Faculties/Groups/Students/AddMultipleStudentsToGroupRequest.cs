namespace InspireEd.Presentation.Contracts.DepartmentHeads.Faculties.Groups.Students;

public sealed record AddMultipleStudentsToGroupRequest(
    List<AddStudentToGroup> Students);
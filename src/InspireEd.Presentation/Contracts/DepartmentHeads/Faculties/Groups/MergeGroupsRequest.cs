namespace InspireEd.Presentation.Contracts.DepartmentHeads.Faculties.Groups;

public sealed record MergeGroupsRequest(
    List<Guid> GroupIds);
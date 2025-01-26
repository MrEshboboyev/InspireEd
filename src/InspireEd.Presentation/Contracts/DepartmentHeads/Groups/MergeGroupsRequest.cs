namespace InspireEd.Presentation.Contracts.DepartmentHeads.Groups;

public sealed record MergeGroupsRequest(
    List<Guid> GroupIds);
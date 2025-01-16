namespace InspireEd.Presentation.Contracts.DepartmentHeads.Classes;

public sealed record UpdateGroupIdsRequest(
    List<Guid> GroupIds);
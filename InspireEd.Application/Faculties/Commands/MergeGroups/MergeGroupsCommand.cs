using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Commands.MergeGroups;

public sealed record MergeGroupsCommand(
    Guid FacultyId,
    List<Guid> GroupIds) : ICommand;
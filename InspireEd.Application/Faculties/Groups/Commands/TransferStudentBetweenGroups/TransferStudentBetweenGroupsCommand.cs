using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Groups.Commands.TransferStudentBetweenGroups;

public sealed record TransferStudentBetweenGroupsCommand(
    Guid FacultyId,
    Guid SourceGroupId,
    Guid TargetGroupId,
    Guid StudentId) : ICommand;
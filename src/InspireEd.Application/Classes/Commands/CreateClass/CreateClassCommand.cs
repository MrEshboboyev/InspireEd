using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Classes.Enums;

namespace InspireEd.Application.Classes.Commands.CreateClass;

public sealed record CreateClassCommand(
    Guid SubjectId,
    Guid TeacherId,
    ClassType ClassType,
    List<Guid> GroupIds,
    DateTime ScheduledDate) : ICommand;
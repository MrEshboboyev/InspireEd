using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Classes.Enums;

namespace InspireEd.Application.Classes.Commands.UpdateClass;

public sealed record UpdateClassCommand(
    Guid ClassId,
    Guid SubjectId,
    Guid TeacherId,
    ClassType Type,
    DateTime ScheduledDate) : ICommand;
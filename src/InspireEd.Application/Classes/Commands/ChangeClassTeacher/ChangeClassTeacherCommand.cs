using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Classes.Commands.ChangeClassTeacher;

public sealed record ChangeClassTeacherCommand(
    Guid ClassId,
    Guid TeacherId) : ICommand;
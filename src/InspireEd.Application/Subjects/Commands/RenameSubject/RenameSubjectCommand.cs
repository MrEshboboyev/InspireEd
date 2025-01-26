using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Subjects.Commands.RenameSubject;

public sealed record RenameSubjectCommand(
    Guid Id,
    string NewName) : ICommand;
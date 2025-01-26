using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Subjects.Commands.UpdateSubject;

public sealed record UpdateSubjectCommand(
    Guid Id,
    string Name,
    string Code,
    int Credit) : ICommand;
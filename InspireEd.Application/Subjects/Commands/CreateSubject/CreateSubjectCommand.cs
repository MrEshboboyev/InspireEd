using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Subjects.Commands.CreateSubject;

public sealed record CreateSubjectCommand(
    string Name,
    string Code,
    int Credit) : ICommand;

using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Subjects.Commands.DeleteSubject;

public sealed record DeleteSubjectCommand(
    Guid SubjectId) : ICommand;
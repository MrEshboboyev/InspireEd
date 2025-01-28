using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Subjects.Commands.ChangeSubjectCode;

public sealed record ChangeSubjectCodeCommand(
    Guid Id,
    string NewCode) : ICommand;
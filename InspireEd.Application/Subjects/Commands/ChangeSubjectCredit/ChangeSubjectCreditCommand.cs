using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Subjects.Commands.ChangeSubjectCredit;

public sealed record ChangeSubjectCreditCommand(
    Guid Id,
    int NewCredit) : ICommand;
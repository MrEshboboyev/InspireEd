using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Classes.Commands.RescheduleClass;

public sealed record RescheduleClassCommand(
    Guid ClassId,
    DateTime NewScheduledDate) : ICommand;
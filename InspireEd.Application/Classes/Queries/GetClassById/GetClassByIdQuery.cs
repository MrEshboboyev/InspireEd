using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Classes.Queries.GetClassById;

public sealed record GetClassByIdQuery(
    Guid Id) : IQuery<ClassResponse>;
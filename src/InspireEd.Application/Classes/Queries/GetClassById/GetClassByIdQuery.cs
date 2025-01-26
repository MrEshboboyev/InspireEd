using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Classes.Queries.Common;

namespace InspireEd.Application.Classes.Queries.GetClassById;

public sealed record GetClassByIdQuery(
    Guid Id) : IQuery<ClassResponse>;
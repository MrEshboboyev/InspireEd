using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Subjects.Queries.Common;

namespace InspireEd.Application.Subjects.Queries.GetSubjectById;

public sealed record GetSubjectByIdQuery(
    Guid SubjectId) : IQuery<SubjectResponse>;
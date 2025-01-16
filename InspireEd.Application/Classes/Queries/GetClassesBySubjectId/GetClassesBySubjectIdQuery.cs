using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Classes.Queries.Common;

namespace InspireEd.Application.Classes.Queries.GetClassesBySubjectId;

public sealed record GetClassesBySubjectIdQuery(
    Guid SubjectId) : IQuery<List<ClassResponse>>;
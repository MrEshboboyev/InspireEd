using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Subjects.Queries.Common;

namespace InspireEd.Application.Subjects.Queries.GetSubjectsByCreditRange;

public sealed record GetSubjectsByCreditRangeQuery(
    int MinCredit,
    int MaxCredit) : IQuery<List<SubjectResponse>>;
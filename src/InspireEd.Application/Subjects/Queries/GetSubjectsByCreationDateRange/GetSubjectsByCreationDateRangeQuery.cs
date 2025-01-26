using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Subjects.Queries.Common;

namespace InspireEd.Application.Subjects.Queries.GetSubjectsByCreationDateRange;

public sealed record GetSubjectsByCreationDateRangeQuery(
    DateTime StartDate,
    DateTime EndDate) : IQuery<List<SubjectResponse>>;
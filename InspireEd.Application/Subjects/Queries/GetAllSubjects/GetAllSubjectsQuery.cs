using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Subjects.Queries.Common;

namespace InspireEd.Application.Subjects.Queries.GetAllSubjects;

public sealed record GetAllSubjectsQuery : IQuery<List<SubjectResponse>>;
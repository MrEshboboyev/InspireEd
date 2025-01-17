using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Faculties.Groups.Queries.Common;

namespace InspireEd.Application.Faculties.Groups.Queries.GetStudentDetails;

public sealed record GetStudentDetailsQuery(
    Guid StudentId) : IQuery<StudentResponse>;
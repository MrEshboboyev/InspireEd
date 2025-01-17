using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Faculties.Groups.Queries.Common;

namespace InspireEd.Application.Faculties.Groups.Queries.GetAllStudentsInFaculty;

public sealed record GetAllStudentsInFacultyQuery(
    Guid FacultyId) : IQuery<List<StudentResponse>>;
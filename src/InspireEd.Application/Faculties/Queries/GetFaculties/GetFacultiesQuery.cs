using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Faculties.Queries.Common;

namespace InspireEd.Application.Faculties.Queries.GetFaculties;

public sealed record GetFacultiesQuery() : IQuery<FacultyListResponse>;
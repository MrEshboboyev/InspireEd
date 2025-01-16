using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Faculties.Queries.Common;

namespace InspireEd.Application.Faculties.Queries.GetFacultyDetails;

public sealed record GetFacultyDetailsQuery(
    Guid FacultyId) : IQuery<FacultyResponse>;
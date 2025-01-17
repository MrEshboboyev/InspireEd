using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Faculties.DepartmentHeads.Queries.Common;

namespace InspireEd.Application.Faculties.Queries.GetAllDepartmentHeadsInFaculty;

public sealed record GetAllDepartmentHeadsInFacultyQuery(
    Guid FacultyId) : IQuery<List<DepartmentHeadResponse>>;
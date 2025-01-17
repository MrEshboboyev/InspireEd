using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Faculties.Queries.Common;

namespace InspireEd.Application.Faculties.Groups.Queries.GetAllGroupsInFaculty;

public sealed record GetAllGroupsInFacultyQuery(
    Guid FacultyId) : IQuery<List<GroupResponse>>;
using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Faculties.Queries.Common;

namespace InspireEd.Application.Faculties.Groups.Queries.GetGroupDetails;

public sealed record GetGroupDetailsQuery(
    Guid GroupId) : IQuery<GroupResponse>;
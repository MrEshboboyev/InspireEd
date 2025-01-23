using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Users.Queries.Common;

namespace InspireEd.Application.Users.Queries.GetUserRoles;

public sealed record GetUserRolesQuery(
    Guid UserId) : IQuery<List<RoleResponse>>;
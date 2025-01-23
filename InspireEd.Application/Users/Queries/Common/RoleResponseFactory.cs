using InspireEd.Domain.Users.Entities;

namespace InspireEd.Application.Users.Queries.Common;

public static class RoleResponseFactory
{
    public static RoleResponse Create(Role role)
    {
        return new RoleResponse(
            role.Id,
            role.Name);
    }
}
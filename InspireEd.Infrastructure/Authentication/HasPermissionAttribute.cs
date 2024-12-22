using InspireEd.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace InspireEd.Infrastructure.Authentication;

/// <summary>
/// Custom authorization attribute to check if a user has a specific permission.
/// </summary>
/// <param name="permission">The permission required to access the resource.</param>
public sealed class HasPermissionAttribute(Permission permission) :
    AuthorizeAttribute(policy: permission.ToString() ?? string.Empty)
{
}
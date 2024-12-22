using InspireEd.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace InspireEd.Infrastructure.Authentication;

public sealed class HasPermissionAttribute(Permission permission) : 
    AuthorizeAttribute(policy: permission.ToString())
{
}

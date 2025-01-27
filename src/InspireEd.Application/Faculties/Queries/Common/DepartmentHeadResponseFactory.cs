using InspireEd.Domain.Users.Entities;

namespace InspireEd.Application.Faculties.Queries.Common;

public static class DepartmentHeadResponseFactory
{
    public static DepartmentHeadResponse Create(User departmentHead)
    {
        return new DepartmentHeadResponse(
            departmentHead.Id,
            departmentHead.FirstName.Value,
            departmentHead.LastName.Value,
            departmentHead.Email.Value,
            departmentHead.CreatedOnUtc,
            departmentHead.ModifiedOnUtc); // Adjust faculty ID as necessary
    }
}
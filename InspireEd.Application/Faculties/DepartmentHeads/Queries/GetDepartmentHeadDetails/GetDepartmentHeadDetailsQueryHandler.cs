using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Faculties.DepartmentHeads.Queries.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Entities;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.Faculties.DepartmentHeads.Queries.GetDepartmentHeadDetails;

internal sealed class GetDepartmentHeadDetailsQueryHandler(
    IUserRepository userRepository) : IQueryHandler<GetDepartmentHeadDetailsQuery, DepartmentHeadResponse>
{
    public async Task<Result<DepartmentHeadResponse>> Handle(
        GetDepartmentHeadDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var departmentHeadId = request.DepartmentHeadId;

        #region Get this Department Head

        var user = await userRepository.GetByIdAsync(departmentHeadId, cancellationToken);
        if (user == null || user.Roles.All(r => r != Role.DepartmentHead))
        {
            return Result.Failure<DepartmentHeadResponse>(
                DomainErrors.DepartmentHead.NotFound(departmentHeadId));
        }

        #endregion

        #region Prepare Response

        var departmentHeadResponse = DepartmentHeadResponseFactory.Create(user);

        #endregion

        return Result.Success(departmentHeadResponse);
    }
}
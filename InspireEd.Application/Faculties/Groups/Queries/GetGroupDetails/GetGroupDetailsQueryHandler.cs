using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Faculties.Queries.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Faculties.Groups.Queries.GetGroupDetails;

internal sealed class GetGroupDetailsQueryHandler(
    IGroupRepository groupRepository) : IQueryHandler<GetGroupDetailsQuery, GroupResponse>
{
    public async Task<Result<GroupResponse>> Handle(
        GetGroupDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var groupId = request.GroupId;
        
        var group = await groupRepository.GetByIdAsync(
            groupId,
            cancellationToken);
        if (group == null)
        {
            return Result.Failure<GroupResponse>(
                DomainErrors.Group.NotFound(groupId));
        }

        var groupResponse = GroupResponseFactory.Create(group);

        return Result.Success(groupResponse);
    }
}
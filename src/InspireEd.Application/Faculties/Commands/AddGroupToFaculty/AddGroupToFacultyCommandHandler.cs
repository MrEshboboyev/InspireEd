using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Faculties.Commands.AddGroupToFaculty;

internal sealed class AddGroupToFacultyCommandHandler(
    IFacultyRepository facultyRepository,
    IGroupRepository groupRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddGroupToFacultyCommand>
{
    public async Task<Result> Handle(
        AddGroupToFacultyCommand request,
        CancellationToken cancellationToken)
    {
        var (facultyId, groupName) = request;
        
        #region Get this Faculty
        
        var faculty = await facultyRepository.GetByIdWithGroupsAsync(
            facultyId,
            cancellationToken);
        if (faculty is null)
        {
            return Result.Failure(
                DomainErrors.Faculty.NotFound(facultyId));
        }
        
        #endregion
        
        #region Prepare value objects
        
        var createGroupNameResult = GroupName.Create(groupName);
        if (createGroupNameResult.IsFailure)
        {
            return Result.Failure(
                createGroupNameResult.Error);
        }
        
        #endregion
        
        #region Add group to faculty

        var addGroupResult = faculty.AddGroup(
            Guid.NewGuid(),
            createGroupNameResult.Value);
        if (addGroupResult.IsFailure)
        {
            return Result.Failure(
                addGroupResult.Error);
        }

        #endregion
        
        #region Add and update database
        
        groupRepository.Add(addGroupResult.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}
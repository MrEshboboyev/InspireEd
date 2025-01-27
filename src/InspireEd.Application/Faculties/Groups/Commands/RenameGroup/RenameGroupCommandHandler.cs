using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Faculties.Groups.Commands.RenameGroup;

internal sealed class RenameGroupCommandHandler(
    IFacultyRepository facultyRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RenameGroupCommand>
{
    public async Task<Result> Handle(
        RenameGroupCommand request,
        CancellationToken cancellationToken)
    {
        var (facultyId, groupId, groupName) = request;
        
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
        
        #region Update group

        var updateGroupResult = faculty.UpdateGroup(
            groupId,
            createGroupNameResult.Value);
        if (updateGroupResult.IsFailure)
        {
            return Result.Failure(
                updateGroupResult.Error);
        }

        #endregion
        
        #region Update database
        
        facultyRepository.Update(faculty);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}
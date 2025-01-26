using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Faculties.Groups.Commands.UpdateGroup;

internal sealed class UpdateGroupCommandHandler(
    IFacultyRepository facultyRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateGroupCommand>
{
    private readonly IFacultyRepository _facultyRepository = facultyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        UpdateGroupCommand request,
        CancellationToken cancellationToken)
    {
        var (facultyId, groupId, groupName) = request;
        
        #region Get this Faculty
        
        var faculty = await _facultyRepository.GetByIdAsync(
            facultyId,
            cancellationToken);
        if (faculty == null)
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
        
        _facultyRepository.Update(faculty);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}
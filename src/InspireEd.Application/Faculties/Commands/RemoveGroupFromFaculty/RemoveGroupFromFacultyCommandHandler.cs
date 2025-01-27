using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Faculties.Commands.RemoveGroupFromFaculty;

internal sealed class RemoveGroupFromFacultyCommandHandler(
    IFacultyRepository facultyRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RemoveGroupFromFacultyCommand>
{
    public async Task<Result> Handle(
        RemoveGroupFromFacultyCommand request,
        CancellationToken cancellationToken)
    {
        var (facultyId, groupId) = request;
        
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
        
        #region Remove group from faculty

        var removeGroupResult = faculty.RemoveGroup(
            groupId);
        if (removeGroupResult.IsFailure)
        {
            return Result.Failure(
                removeGroupResult.Error);
        }

        #endregion
        
        #region Update database
        
        facultyRepository.Update(faculty);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}
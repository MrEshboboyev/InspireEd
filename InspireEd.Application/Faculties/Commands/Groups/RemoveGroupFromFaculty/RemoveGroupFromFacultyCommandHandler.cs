using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Faculties.Commands.Groups.RemoveGroupFromFaculty;

internal sealed class RemoveGroupFromFacultyCommandHandler(
    IFacultyRepository facultyRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RemoveGroupFromFacultyCommand>
{
    private readonly IFacultyRepository _facultyRepository = facultyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        RemoveGroupFromFacultyCommand request,
        CancellationToken cancellationToken)
    {
        var (facultyId, groupId) = request;
        
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
        
        _facultyRepository.Update(faculty);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}
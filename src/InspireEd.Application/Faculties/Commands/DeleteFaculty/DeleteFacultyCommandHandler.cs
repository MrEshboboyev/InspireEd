using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Faculties.Commands.DeleteFaculty;

internal sealed class DeleteFacultyCommandHandler(
    IFacultyRepository facultyRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteFacultyCommand>
{
    public async Task<Result> Handle(
        DeleteFacultyCommand request,
        CancellationToken cancellationToken)
    {
        var facultyId = request.FacultyId;
        
        #region Get this faculty
        
        var faculty = await facultyRepository.GetByIdAsync(
            facultyId,
            cancellationToken);
        if (faculty is null)
        {
            return Result.Failure(
                DomainErrors.Faculty.NotFound(facultyId));
        }
        
        #endregion
        
        #region Update database
        
        facultyRepository.Remove(faculty);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}
using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Faculties.Commands.RenameFaculty;

internal sealed class RenameFacultyCommandHandler(
    IFacultyRepository facultyRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RenameFacultyCommand>
{
    public async Task<Result> Handle(
        RenameFacultyCommand request,
        CancellationToken cancellationToken)
    {
        var (facultyId, facultyName) = request;
        
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
        
        #region Prepare value objects
        
        var createFacultyNameResult = FacultyName.Create(facultyName);
        if (createFacultyNameResult.IsFailure)
        {
            return Result.Failure(
                createFacultyNameResult.Error);
        }
        
        #endregion
        
        #region Update this faculty name
        
        var updateFacultyNameResult = faculty.UpdateName(createFacultyNameResult.Value);
        if (updateFacultyNameResult.IsFailure)
        {
            return Result.Failure(
                updateFacultyNameResult.Error);
        }
        
        #endregion
        
        #region Update database
        
        facultyRepository.Update(faculty);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}
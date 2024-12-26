using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Faculties.Commands.UpdateFaculty;

internal sealed class UpdateFacultyCommandHandler(
    IFacultyRepository facultyRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateFacultyCommand>
{
    private readonly IFacultyRepository _facultyRepository = facultyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task<Result> Handle(
        UpdateFacultyCommand request,
        CancellationToken cancellationToken)
    {
        var (facultyId, facultyName) = request;
        
        #region Get this faculty
        
        var faculty = await _facultyRepository.GetByIdAsync(
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
        
        #region Update this faculty
        
        faculty.UpdateName(createFacultyNameResult.Value);
        
        #endregion
        
        #region Update database
        
        _facultyRepository.Update(faculty);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}
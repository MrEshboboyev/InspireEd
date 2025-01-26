using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Faculties.Commands.CreateFaculty;

internal sealed class CreateFacultyCommandHandler(
    IFacultyRepository facultyRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateFacultyCommand>
{
    private readonly IFacultyRepository _facultyRepository = facultyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task<Result> Handle(
        CreateFacultyCommand request,
        CancellationToken cancellationToken)
    {
        var facultyName = request.FacultyName;
        
        #region Prepare Value objects

        var createFacultyNameResult = FacultyName.Create(facultyName);
        if (createFacultyNameResult.IsFailure)
        {
            return Result.Failure(
                createFacultyNameResult.Error);
        }

        #endregion
        
        #region Create faculty
        
        var faculty = Faculty.Create(
            Guid.NewGuid(),
            createFacultyNameResult.Value);
        
        #endregion
        
        #region Add and Update database
        
        _facultyRepository.Add(faculty);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}
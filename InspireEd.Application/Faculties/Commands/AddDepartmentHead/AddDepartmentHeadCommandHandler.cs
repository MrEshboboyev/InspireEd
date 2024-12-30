using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.Faculties.Commands.AddDepartmentHead;

internal sealed class AddDepartmentHeadCommandHandler(
    IFacultyRepository facultyRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddDepartmentHeadCommand>
{
    private readonly IFacultyRepository _facultyRepository = facultyRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task<Result> Handle(
        AddDepartmentHeadCommand request,
        CancellationToken cancellationToken)
    {
        var (facultyId, departmentHeadId) = request;
        
        #region Get Faculty and Department Head
        
        var faculty = await _facultyRepository.GetByIdAsync(
            facultyId,
            cancellationToken);
        if (faculty is null)
        {
            return Result.Failure(
                DomainErrors.Faculty.NotFound(facultyId));
        }
        
        var departmentHead = await _userRepository.GetByIdAsync(
            departmentHeadId,
            cancellationToken);
        if (departmentHead is null)
        {
            return Result.Failure(
                DomainErrors.User.NotFound(departmentHeadId));
        }
        
        #endregion
        
        #region Add this department head to faculty

        var addDepartmentHeadResult = faculty.AddDepartmentHead(departmentHeadId);
        if (addDepartmentHeadResult.IsFailure)
        {
            return Result.Failure(
                addDepartmentHeadResult.Error);
        }

        #endregion
        
        #region Update database
        
        _facultyRepository.Update(faculty);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}
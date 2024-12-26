using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.Faculties.Commands.DepartmentHeads.RemoveDepartmentHeadFromFaculty;

internal sealed class RemoveDepartmentHeadFromFacultyCommandHandler(
    IFacultyRepository facultyRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DepartmentHeads.RemoveDepartmentHead.RemoveDepartmentHeadFromFacultyCommand>
{
    private readonly IFacultyRepository _facultyRepository = facultyRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task<Result> Handle(
        DepartmentHeads.RemoveDepartmentHead.RemoveDepartmentHeadFromFacultyCommand request,
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
        
        #region Remove this department head from faculty

        var removeDepartmentHeadResult = faculty.RemoveDepartmentHead(departmentHeadId);
        if (removeDepartmentHeadResult.IsFailure)
        {
            return Result.Failure(
                removeDepartmentHeadResult.Error);
        }

        #endregion
        
        #region Update database
        
        _facultyRepository.Update(faculty);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}
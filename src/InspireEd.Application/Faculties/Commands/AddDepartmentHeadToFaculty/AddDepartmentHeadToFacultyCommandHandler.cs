using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Entities;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.Faculties.Commands.AddDepartmentHeadToFaculty;

internal sealed class AddDepartmentHeadToFacultyCommandHandler(
    IFacultyRepository facultyRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddDepartmentHeadToFacultyCommand>
{
    public async Task<Result> Handle(
        AddDepartmentHeadToFacultyCommand request,
        CancellationToken cancellationToken)
    {
        var (facultyId, departmentHeadId) = request;
        
        #region Get Faculty and Department Head
        
        var faculty = await facultyRepository.GetByIdAsync(
            facultyId,
            cancellationToken);
        if (faculty is null)
        {
            return Result.Failure(
                DomainErrors.Faculty.NotFound(facultyId));
        }
        
        var departmentHead = await userRepository.GetByIdWithRolesAsync(
            departmentHeadId,
            cancellationToken);
        if (departmentHead is null || !departmentHead.IsInRole(Role.DepartmentHead))
        {
            return Result.Failure(
                DomainErrors.DepartmentHead.NotFound(departmentHeadId));
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
        
        facultyRepository.Update(faculty);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}
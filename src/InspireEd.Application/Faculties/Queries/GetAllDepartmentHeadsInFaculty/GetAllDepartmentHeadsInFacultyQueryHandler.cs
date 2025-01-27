using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Faculties.Queries.Common;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.Faculties.Queries.GetAllDepartmentHeadsInFaculty;

internal sealed class GetAllDepartmentHeadsInFacultyQueryHandler(
    IFacultyRepository facultyRepository,
    IUserRepository userRepository) : IQueryHandler<GetAllDepartmentHeadsInFacultyQuery, List<DepartmentHeadResponse>>
{
    public async Task<Result<List<DepartmentHeadResponse>>> Handle(
        GetAllDepartmentHeadsInFacultyQuery request,
        CancellationToken cancellationToken)
    {
        var facultyId = request.FacultyId;
        
        var faculty = await facultyRepository.GetByIdAsync(
            facultyId,
            cancellationToken);
        if (faculty is null)
        {
            return Result.Failure<List<DepartmentHeadResponse>>(
                DomainErrors.Faculty.NotFound(facultyId));
        }

        var departmentHeadIds = faculty.DepartmentHeadIds.ToList();

        var departmentHeads = await userRepository.GetByIdsAsync(
            departmentHeadIds, 
            cancellationToken);
        
        var departmentHeadResponses = departmentHeads
            .Select(DepartmentHeadResponseFactory.Create)
            .ToList();

        return Result.Success(departmentHeadResponses);
    }
}
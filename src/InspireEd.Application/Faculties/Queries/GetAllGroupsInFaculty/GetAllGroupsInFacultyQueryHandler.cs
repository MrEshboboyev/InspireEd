using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Faculties.Queries.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Faculties.Groups.Queries.GetAllGroupsInFaculty;

internal sealed class GetAllGroupsInFacultyQueryHandler(
    IFacultyRepository facultyRepository) : IQueryHandler<GetAllGroupsInFacultyQuery, List<GroupResponse>>
{
    public async Task<Result<List<GroupResponse>>> Handle(
        GetAllGroupsInFacultyQuery request,
        CancellationToken cancellationToken)
    {
        var facultyId = request.FacultyId;
        
        var faculty = await facultyRepository.GetByIdAsync(
            facultyId,
            cancellationToken);
        if (faculty is null)
        {
            return Result.Failure<List<GroupResponse>>(
                DomainErrors.Faculty.NotFound(facultyId));
        }

        var groupResponses = faculty.Groups
            .Select(GroupResponseFactory.Create)
            .ToList();

        return Result.Success(groupResponses);
    }
}
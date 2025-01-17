using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Faculties.Groups.Queries.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.Faculties.Groups.Queries.GetAllStudentsInFaculty;

internal sealed class GetAllStudentsInFacultyQueryHandler(
    IFacultyRepository facultyRepository,
    IUserRepository userRepository) : IQueryHandler<GetAllStudentsInFacultyQuery, List<StudentResponse>>
{
    public async Task<Result<List<StudentResponse>>> Handle(
        GetAllStudentsInFacultyQuery request,
        CancellationToken cancellationToken)
    {
        var facultyId = request.FacultyId;

        #region Get this Faculty 

        var faculty = await facultyRepository.GetByIdAsync(facultyId, cancellationToken);
        if (faculty == null)
        {
            return Result.Failure<List<StudentResponse>>(
                DomainErrors.Faculty.NotFound(facultyId));
        }

        #endregion

        #region Get students

        var studentIds = faculty.Groups.SelectMany(group => group.StudentIds).ToList();
        var students = await userRepository.GetByIdsAsync(
            studentIds, 
            cancellationToken);

        var studentResponses = students
            .Select(StudentResponseFactory.Create)
            .ToList();

        #endregion

        return Result.Success(studentResponses);
    }
}
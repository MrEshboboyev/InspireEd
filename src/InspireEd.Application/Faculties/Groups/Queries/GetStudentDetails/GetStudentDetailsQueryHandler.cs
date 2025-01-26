using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Faculties.Groups.Queries.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.Faculties.Groups.Queries.GetStudentDetails;

internal sealed class GetStudentDetailsQueryHandler(
    IUserRepository userRepository) : IQueryHandler<GetStudentDetailsQuery, StudentResponse>
{
    public async Task<Result<StudentResponse>> Handle(
        GetStudentDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var studentId = request.StudentId;
        
        #region Get this Student
        
        var student = await userRepository.GetByIdAsync(
            studentId,
            cancellationToken);
        if (student == null)
        {
            return Result.Failure<StudentResponse>(
                DomainErrors.Student.NotFound(studentId));
        }
        
        #endregion
        
        #region Prepare response

        var studentResponse = StudentResponseFactory.Create(student);
        
        #endregion

        return Result.Success(studentResponse);
    }
}
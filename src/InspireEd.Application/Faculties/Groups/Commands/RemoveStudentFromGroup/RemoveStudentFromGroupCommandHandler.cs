using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.Faculties.Groups.Commands.RemoveStudentFromGroup;

internal sealed class RemoveStudentFromGroupCommandHandler(
    IFacultyRepository facultyRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RemoveStudentFromGroupCommand>
{
    public async Task<Result> Handle(
        RemoveStudentFromGroupCommand request, 
        CancellationToken cancellationToken)
    {
        var (facultyId, groupId, studentId) = request;
        
        #region Get faculty, group and student
        
        var faculty = await facultyRepository.GetByIdWithGroupsAsync(
            facultyId,
            cancellationToken);
        if (faculty is null)
        {
            return Result.Failure(
                DomainErrors.Faculty.NotFound(facultyId));
        }
        
        var group = faculty.GetGroupById(groupId);
        if (group is null)
        {
            return Result.Failure(
                DomainErrors.Group.NotFound(groupId));
        }
        
        var removeStudentFromGroupResult = group.RemoveStudent(studentId);
        if (removeStudentFromGroupResult.IsFailure)
        {
            return Result.Failure(
                removeStudentFromGroupResult.Error);
        }
        
        #endregion
        
        #region Get this user (student) from db
        
        var student = await userRepository.GetByIdAsync(
            studentId,
            cancellationToken);
        if (student is null)
        {
            return Result.Failure(
                DomainErrors.User.NotFound(studentId));
        }
        
        #endregion
        
        #region Remove and update database
        
        userRepository.Delete(student);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion
        
        return Result.Success();
    }
}
using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Entities;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.Classes.Commands.ChangeClassTeacher;

internal sealed class ChangeClassTeacherCommandHandler(
    IClassRepository classRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<ChangeClassTeacherCommand>
{
    public async Task<Result> Handle(
        ChangeClassTeacherCommand request,
        CancellationToken cancellationToken)
    {
        var (classId, teacherId) = request;
        
        #region Get this Subject and Teacher
        
        var classEntity = await classRepository.GetByIdAsync(
            classId,
            cancellationToken);
        if (classEntity is null)
        {
            return Result.Failure(
                DomainErrors.Class.NotFound(classId));
        }
        
        var teacher = await userRepository.GetByIdAsync(
            teacherId,
            cancellationToken);
        if (teacher is null || !teacher.IsInRole(Role.Teacher))
        {
            return Result.Failure(
                DomainErrors.Teacher.NotFound(teacherId));
        }
        
        #endregion
        
        #region Update teacher in this Subject
        
        var changeTeacherResult = classEntity.ChangeTeacher(teacherId);
        if (changeTeacherResult.IsFailure)
        {
            return Result.Failure(
                changeTeacherResult.Error);
        }
        
        #endregion
        
        #region Update database
        
        classRepository.Update(classEntity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}
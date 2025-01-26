using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Classes.Entities;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Subjects.Repositories;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.Classes.Commands.CreateClass;

/// <summary>
/// Command handler for creating a new class.
/// </summary>
internal sealed class CreateClassCommandHandler(
    ISubjectRepository subjectRepository,
    IUserRepository userRepository,
    IGroupRepository groupRepository,
    IClassRepository classRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateClassCommand>
{
    public async Task<Result> Handle(
        CreateClassCommand request,
        CancellationToken cancellationToken)
    {
        var (subjectId, teacherId, 
            classType, groupIds, scheduledDate) = request;

        #region Get Subject, Teacher, and Groups

        // Get Subject
        var subject = await subjectRepository.GetByIdAsync(
            subjectId,
            cancellationToken);
        if (subject is null)
        {
            return Result.Failure(
                DomainErrors.Subject.NotFound(subjectId));
        }

        // Get Teacher
        var teacher = await userRepository.GetByIdAsync(
            teacherId,
            cancellationToken);
        if (teacher is null)
        {
            return Result.Failure(
                DomainErrors.Teacher.NotFound(teacherId));
        }

        // Get Groups
        var groups = await groupRepository.GetByIdsAsync(
            groupIds,
            cancellationToken);

        // Ensure all requested groups exist
        var missingGroups = groupIds
            .Except(groups.Select(g => g.Id))
            .ToList();
        if (missingGroups.Count != 0)
        {
            return Result.Failure(
                DomainErrors.Group.MissingGroups(missingGroups));
        }

        #endregion

        #region Prepare Value Objects

        // Prepare a list of group IDs
        var groupIdList = groups
            .Select(g => g.Id)
            .ToList();

        #endregion

        #region Create New Class

        var newClass = Class.Create(
            Guid.NewGuid(),  // Generate a new unique ID for the class
            subject.Id,
            teacher.Id,
            classType,
            groupIdList,
            scheduledDate);

        #endregion

        #region Add and Update Database

        // Add the new class to the repository
        classRepository.Add(newClass);

        // Save changes using Unit of Work
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}

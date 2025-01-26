using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Subjects.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Subjects.ValueObjects;

namespace InspireEd.Application.Subjects.Commands.RenameSubject;

internal sealed class RenameSubjectCommandHandler(
    ISubjectRepository subjectRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RenameSubjectCommand>
{
    public async Task<Result> Handle(
        RenameSubjectCommand request,
        CancellationToken cancellationToken)
    {
        var (subjectId, newName) = request;
        
        #region Get this subject
        
        var subject = await subjectRepository.GetByIdAsync(
            subjectId,
            cancellationToken);
        if (subject == null)
        {
            return Result.Failure(
                DomainErrors.Subject.NotFound(request.Id));
        }
        
        #endregion
        
        #region Prepare value objects

        var subjectNameResult = SubjectName.Create(request.NewName);
        if (subjectNameResult.IsFailure)
        {
            return Result.Failure(
                subjectNameResult.Error);
        }
        
        #endregion

        #region Update this subject name
        
        var updateSubjectNameResult = subject.Rename(subjectNameResult.Value);
        if (updateSubjectNameResult.IsFailure)
        {
            return Result.Failure(
                updateSubjectNameResult.Error);
        }
        
        #endregion

        #region Update database
        
        subjectRepository.Update(subject);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}
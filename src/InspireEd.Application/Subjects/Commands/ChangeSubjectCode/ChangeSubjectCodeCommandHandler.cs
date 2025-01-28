using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Subjects.Repositories;
using InspireEd.Domain.Subjects.ValueObjects;

namespace InspireEd.Application.Subjects.Commands.ChangeSubjectCode;

internal sealed class ChangeSubjectCodeCommandHandler(
    ISubjectRepository subjectRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<ChangeSubjectCodeCommand>
{
    public async Task<Result> Handle(
        ChangeSubjectCodeCommand request,
        CancellationToken cancellationToken)
    {
        var (subjectId, newCode) = request;
        
        #region Get this Subject
        
        var subject = await subjectRepository.GetByIdAsync(subjectId, cancellationToken);
        if (subject is null)
        {
            return Result.Failure(
                DomainErrors.Subject.NotFound(subjectId));
        }
        
        #endregion

        #region Prepare value objects 
        
        var subjectCodeResult = SubjectCode.Create(newCode);
        if (subjectCodeResult.IsFailure)
        {
            return Result.Failure(
                subjectCodeResult.Error);
        }
        
        #endregion
        
        #region Checking this Subject Code is unique
        
        if (!await subjectRepository.IsCodeUniqueAsync(subjectCodeResult.Value, cancellationToken))
        {
            return Result.Failure(
                DomainErrors.Subject.CodeAlreadyInUse);
        }
        
        #endregion

        #region Change Code
        
        var changeCodeResult = subject.ChangeCode(subjectCodeResult.Value);
        if (changeCodeResult.IsFailure)
        {
            return Result.Failure(
                changeCodeResult.Error);
        }

        #endregion
        
        #region Update database
        
        subjectRepository.Update(subject);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}
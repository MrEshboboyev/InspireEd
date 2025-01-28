using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Subjects.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Subjects.ValueObjects;

namespace InspireEd.Application.Subjects.Commands.UpdateSubject;

internal sealed class UpdateSubjectCommandHandler(
    ISubjectRepository subjectRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateSubjectCommand>
{
    public async Task<Result> Handle(
        UpdateSubjectCommand request,
        CancellationToken cancellationToken)
    {
        var (subjectId, name, code, credit) = request;
        
        #region Get this Subject
        
        var subject = await subjectRepository.GetByIdAsync(
            subjectId,
            cancellationToken);
        if (subject is null)
        {
            return Result.Failure(
                DomainErrors.Subject.NotFound(subjectId));
        }
        
        #endregion
        
        #region Prepare value objects

        var subjectNameResult = SubjectName.Create(name);
        if (subjectNameResult.IsFailure)
        {
            return Result.Failure(
                subjectNameResult.Error);
        }

        var subjectCodeResult = SubjectCode.Create(code);
        if (subjectCodeResult.IsFailure)
        {
            return Result.Failure(
                subjectCodeResult.Error);
        }

        var subjectCreditResult = SubjectCredit.Create(credit);
        if (subjectCreditResult.IsFailure)
        {
            return Result.Failure(
                subjectCreditResult.Error);
        }
        
        #endregion
        
        #region Checking this Subject Name and Code is unique

        if (!await subjectRepository.IsNameUniqueAsync(subjectNameResult.Value, cancellationToken))
        {
            return Result.Failure(
                DomainErrors.Subject.NameAlreadyInUse);
        }
        
        if (!await subjectRepository.IsCodeUniqueAsync(subjectCodeResult.Value, cancellationToken))
        {
            return Result.Failure(
                DomainErrors.Subject.CodeAlreadyInUse);
        }
        
        #endregion
        
        #region Update this Subject

        var updateSubjectDetails = subject.UpdateSubjectDetails(
            subjectNameResult.Value,
            subjectCodeResult.Value,
            subjectCreditResult.Value);
        if (updateSubjectDetails.IsFailure)
        {
            return Result.Failure(
                updateSubjectDetails.Error);
        }
        
        #endregion
        
        #region Update database

        subjectRepository.Update(subject);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}
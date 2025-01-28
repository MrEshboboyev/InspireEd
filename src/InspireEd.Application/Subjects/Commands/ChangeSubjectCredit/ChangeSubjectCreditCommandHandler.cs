using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Subjects.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Subjects.ValueObjects;

namespace InspireEd.Application.Subjects.Commands.ChangeSubjectCredit;

internal sealed class ChangeSubjectCreditCommandHandler(
    ISubjectRepository subjectRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<ChangeSubjectCreditCommand>
{
    public async Task<Result> Handle(
        ChangeSubjectCreditCommand request,
        CancellationToken cancellationToken)
    {
        var (subjectId, newCredit) = request;
        
        #region Get this Subject
        
        var subject = await subjectRepository.GetByIdAsync(subjectId, cancellationToken);
        if (subject is null)
        {
            return Result.Failure(
                DomainErrors.Subject.NotFound(request.Id));
        }
        
        #endregion

        #region Prepare value objects 
        
        var subjectCreditResult = SubjectCredit.Create(newCredit);
        if (subjectCreditResult.IsFailure)
        {
            return Result.Failure(subjectCreditResult.Error);
        }
        
        #endregion

        #region Change credit
        
        var changeCreditResult = subject.ChangeCredit(subjectCreditResult.Value);
        if (changeCreditResult.IsFailure)
        {
            return Result.Failure(
                changeCreditResult.Error);
        }

        #endregion
        
        #region Update database
        
        subjectRepository.Update(subject);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}
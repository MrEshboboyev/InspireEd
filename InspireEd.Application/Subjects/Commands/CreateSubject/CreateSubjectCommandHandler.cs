using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Subjects.Entities;
using InspireEd.Domain.Subjects.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Subjects.ValueObjects;

namespace InspireEd.Application.Subjects.Commands.CreateSubject;

internal sealed class CreateSubjectCommandHandler(
    ISubjectRepository subjectRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateSubjectCommand>
{
    public async Task<Result> Handle(
        CreateSubjectCommand request,
        CancellationToken cancellationToken)
    {
        #region Prepare value objects
        
        var subjectNameResult = SubjectName.Create(request.Name);
        if (subjectNameResult.IsFailure)
        {
            return Result.Failure(subjectNameResult.Error);
        }

        var subjectCodeResult = SubjectCode.Create(request.Code);
        if (subjectCodeResult.IsFailure)
        {
            return Result.Failure(subjectCodeResult.Error);
        }

        var subjectCreditResult = SubjectCredit.Create(request.Credit);
        if (subjectCreditResult.IsFailure)
        {
            return Result.Failure(subjectCreditResult.Error);
        }
        
        #endregion
        
        #region Create new Subject
        
        var subject = Subject.Create(
            Guid.NewGuid(),
            subjectNameResult.Value,
            subjectCodeResult.Value,
            subjectCreditResult.Value);
        
        #endregion

        #region Add and Update database
        
        subjectRepository.Add(subject);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion
        
        return Result.Success();
    }
}
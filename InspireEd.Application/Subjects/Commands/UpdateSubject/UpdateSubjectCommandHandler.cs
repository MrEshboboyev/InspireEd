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
        var subject = await subjectRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (subject == null)
        {
            return Result.Failure(DomainErrors.Subject.NotFound(request.Id));
        }

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

        var updateSubjectDetails = subject.UpdateSubjectDetails(
            subjectNameResult.Value,
            subjectCodeResult.Value,
            subjectCreditResult.Value);
        if (updateSubjectDetails.IsFailure)
        {
            return Result.Failure(
                updateSubjectDetails.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
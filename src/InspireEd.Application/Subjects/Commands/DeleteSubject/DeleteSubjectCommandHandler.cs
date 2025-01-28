using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Subjects.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Subjects.Commands.DeleteSubject;

internal sealed class DeleteSubjectCommandHandler(
    ISubjectRepository subjectRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteSubjectCommand>
{
    public async Task<Result> Handle(
        DeleteSubjectCommand request,
        CancellationToken cancellationToken)
    {
        var subjectId = request.SubjectId;
        
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
        
        #region Delete and Update database

        subjectRepository.Remove(subject);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion
        
        return Result.Success();
    }
}
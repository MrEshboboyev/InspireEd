using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.Faculties.DepartmentHeads.Commands.DeleteDepartmentHead;

internal sealed class DeleteDepartmentHeadCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteDepartmentHeadCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task<Result> Handle(
        DeleteDepartmentHeadCommand request,
        CancellationToken cancellationToken)
    {
        var departmentHeadId = request.DepartmentHeadId;
        
        #region Get this Department Head
        
        var departmentHead = await _userRepository.GetByIdAsync(
            departmentHeadId,
            cancellationToken);
        if (departmentHead is null)
        {
            return Result.Failure(
                DomainErrors.User.NotFound(departmentHeadId));
        }
        
        #endregion
        
        #region Delete this Department Head
        
        _userRepository.Delete(departmentHead);
        
        // Removing this department head's ID from all faculties to
        // which he is assigned will be resolved soon.
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}
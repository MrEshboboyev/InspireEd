using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Repositories;
using InspireEd.Domain.Users.ValueObjects;

namespace InspireEd.Application.Faculties.DepartmentHeads.Commands.UpdateDepartmentHeadDetails;

internal sealed class UpdateDepartmentHeadDetailsCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateDepartmentHeadDetailsCommand>
{
    public async Task<Result> Handle(
        UpdateDepartmentHeadDetailsCommand request,
        CancellationToken cancellationToken)
    {
        var (departmentHeadId, firstName, lastName, email, password) = request;

        #region Get Department Head

        var user = await userRepository.GetByIdAsync(departmentHeadId, cancellationToken);
        if (user is null)
        {
            return Result.Failure(
                DomainErrors.User.NotFound(departmentHeadId));
        }

        #endregion

        #region Create Value Objects and Validate

        var firstNameResult = FirstName.Create(firstName);
        if (firstNameResult.IsFailure)
        {
            return Result.Failure(firstNameResult.Error);
        }

        var lastNameResult = LastName.Create(lastName);
        if (lastNameResult.IsFailure)
        {
            return Result.Failure(lastNameResult.Error);
        }

        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
        {
            return Result.Failure(emailResult.Error);
        }

        #endregion

        #region Update Department Head Details

        var updateResult = user.UpdateDetails(
            firstNameResult.Value,
            lastNameResult.Value,
            emailResult.Value);

        if (updateResult.IsFailure)
        {
            return updateResult;
        }

        #endregion

        #region Save Changes to Database

        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}

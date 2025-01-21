using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Users.Queries.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Repositories;
using InspireEd.Domain.Users.ValueObjects;

namespace InspireEd.Application.Users.Queries.GetUserByEmail;

internal sealed class GetUserByEmailQueryHandler(
    IUserRepository userRepository) : IQueryHandler<GetUserByEmailQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(
        GetUserByEmailQuery request,
        CancellationToken cancellationToken)
    {
        var email = request.Email;
        
        #region Prepare valuer object
        
        var createEmailResult = Email.Create(email);
        if (createEmailResult.IsFailure)
        {
            return Result.Failure<UserResponse>(
                createEmailResult.Error);
        }
        
        #endregion
        
        #region Get User By Email
        
        var user = await userRepository.GetByEmailAsync(
            createEmailResult.Value,
            cancellationToken);
        if (user is null)
        {
            return Result.Failure<UserResponse>(
                DomainErrors.User.NotFoundForEmail(email));
        }
        
        #endregion

        #region Prepare response
        
        var response = UserResponseFactory.Create(user);
        
        #endregion

        return Result.Success(response);
    }
}
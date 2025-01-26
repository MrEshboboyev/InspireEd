using InspireEd.Application.Abstractions.Security;
using InspireEd.Application.Users.Commands.ChangeUserPassword;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Users.Entities;
using InspireEd.Domain.Users.Repositories;
using InspireEd.Domain.Users.ValueObjects;
using Moq;

namespace InspireEd.Application.UnitTests.Users.Commands;

public class UpdateUserPasswordCommandHandlerTests
{
    #region Fields & Mock Setup
    
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly UpdateUserPasswordCommandHandler _handler;

    public UpdateUserPasswordCommandHandlerTests()
    {
        _handler = new UpdateUserPasswordCommandHandler(
            _userRepositoryMock.Object,
            _passwordHasherMock.Object,
            _unitOfWorkMock.Object);
    }
    
    #endregion

    #region Helper methods
    
    // Helper method to create a User instance
    private static User CreateTestUser(Guid id, string email, string passwordHash, string firstName, string lastName, string roleName)
    {
        var emailObj = Email.Create(email).Value;
        var firstNameObj = FirstName.Create(firstName).Value;
        var lastNameObj = LastName.Create(lastName).Value;
        var role = Role.FromName(roleName);

        return User.Create(id, emailObj, passwordHash, firstNameObj, lastNameObj, role);
    }
    
    #endregion
    
    #region Test Methods

    [Fact]
    public async Task Handle_Should_UpdatePassword_Successfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var newPassword = "new-secure-password";
        var hashedPassword = "hashed-new-password";

        var command = new UpdateUserPasswordCommand(userId, newPassword);

        var user = CreateTestUser(
            id: userId,
            email: "user@example.com",
            passwordHash: "old-hashed-password",
            firstName: "John",
            lastName: "Doe",
            roleName: "Admin");

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(hasher => hasher.Hash(newPassword))
            .Returns(hashedPassword);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(hashedPassword, user.PasswordHash);
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Update(user), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_UserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateUserPasswordCommand(userId, "new-secure-password");

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.User.NotFound(userId), result.Error);
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_ChangePasswordFails()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var newPassword = ""; // Invalid password
        var command = new UpdateUserPasswordCommand(userId, newPassword);

        var user = CreateTestUser(
            id: userId,
            email: "user@example.com",
            passwordHash: "old-hashed-password",
            firstName: "John",
            lastName: "Doe",
            roleName: "Admin");

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.User.InvalidPasswordChange, result.Error);
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    #endregion
}
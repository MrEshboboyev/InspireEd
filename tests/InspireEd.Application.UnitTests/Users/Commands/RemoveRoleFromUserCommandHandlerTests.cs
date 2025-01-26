using InspireEd.Application.Users.Commands.RemoveRoleFromUser;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Users.Entities;
using InspireEd.Domain.Users.Repositories;
using InspireEd.Domain.Users.ValueObjects;
using Moq;

namespace InspireEd.Application.UnitTests.Users.Commands;

public class RemoveRoleFromUserCommandHandlerTests
{
    #region Fields & Mock Setup
    
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IRoleRepository> _roleRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly RemoveRoleFromUserCommandHandler _handler;

    public RemoveRoleFromUserCommandHandlerTests()
    {
        _handler = new RemoveRoleFromUserCommandHandler(
            _userRepositoryMock.Object,
            _roleRepositoryMock.Object,
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

    // Helper method to create a Role instance
    private static Role CreateTestRole(int id, string name)
    {
        return new Role(id, name);
    }
    
    #endregion
    
    #region Test Methods

    [Fact]
    public async Task Handle_Should_RemoveRoleFromUser_Successfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var roleId = 1;

        var command = new RemoveRoleFromUserCommand(userId, roleId);

        var role = CreateTestRole(roleId, "Manager");
        var user = CreateTestUser(
            id: userId,
            email: "user@example.com",
            passwordHash: "hashedPassword",
            firstName: "John",
            lastName: "Doe",
            roleName: "Admin");

        user.AssignRole(role); // Assign role to user for testing removal

        _userRepositoryMock
            .Setup(repo => repo.GetByIdWithRolesAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _roleRepositoryMock
            .Setup(repo => repo.GetByIdAsync(roleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _userRepositoryMock.Verify(repo => repo.GetByIdWithRolesAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        _roleRepositoryMock.Verify(repo => repo.GetByIdAsync(roleId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Update(user), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_UserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var roleId = 1;

        var command = new RemoveRoleFromUserCommand(userId, roleId);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdWithRolesAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.User.NotFound(userId), result.Error);
        _userRepositoryMock.Verify(repo => repo.GetByIdWithRolesAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        _roleRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_RoleNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var roleId = 1;

        var command = new RemoveRoleFromUserCommand(userId, roleId);

        var user = CreateTestUser(
            id: userId,
            email: "user@example.com",
            passwordHash: "hashedPassword",
            firstName: "John",
            lastName: "Doe",
            roleName: "Admin");

        _userRepositoryMock
            .Setup(repo => repo.GetByIdWithRolesAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _roleRepositoryMock
            .Setup(repo => repo.GetByIdAsync(roleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Role)null!);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Role.NotFound(roleId), result.Error);
        _userRepositoryMock.Verify(repo => repo.GetByIdWithRolesAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        _roleRepositoryMock.Verify(repo => repo.GetByIdAsync(roleId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_RoleNotAssignedToUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var roleId = 10;

        var command = new RemoveRoleFromUserCommand(userId, roleId);

        var role = CreateTestRole(roleId, "Student");
        var user = CreateTestUser(
            id: userId,
            email: "user@example.com",
            passwordHash: "hashedPassword",
            firstName: "John",
            lastName: "Doe",
            roleName: "Admin");

        _userRepositoryMock
            .Setup(repo => repo.GetByIdWithRolesAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _roleRepositoryMock
            .Setup(repo => repo.GetByIdAsync(roleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.User.RoleNotAssigned(roleId), result.Error);
        _userRepositoryMock.Verify(repo => repo.GetByIdWithRolesAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        _roleRepositoryMock.Verify(repo => repo.GetByIdAsync(roleId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    #endregion
}
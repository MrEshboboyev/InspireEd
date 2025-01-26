using InspireEd.Application.Abstractions.Security;
using InspireEd.Application.Users.Commands.CreateUser;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Users.Entities;
using InspireEd.Domain.Users.Repositories;
using InspireEd.Domain.Users.ValueObjects;
using Moq;

namespace InspireEd.Application.UnitTests.Users.Commands;

public class CreateUserCommandHandlerTests
{
    #region Fields & Mock Setup
    
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IRoleRepository> _roleRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
    
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _handler = new CreateUserCommandHandler(
            _userRepositoryMock.Object,
            _roleRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _passwordHasherMock.Object);
    }
    
    #endregion
    
    #region Test Methods

    [Fact]
    public async Task Handle_Should_CreateUser_Successfully()
    {
        // Arrange
        var command = new CreateUserCommand(
            Email: "user@example.com",
            Password: "securePassword",
            FirstName: "John",
            LastName: "Doe",
            RoleName: "Admin");

        var email = Email.Create(command.Email).Value;

        var role = Role.FromName(command.RoleName);
        var hashedPassword = "hashedPassword";

        _userRepositoryMock
            .Setup(repo => repo.IsEmailUniqueAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _roleRepositoryMock
            .Setup(repo => repo.GetByNameAsync(command.RoleName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        _passwordHasherMock
            .Setup(hasher => hasher.Hash(command.Password))
            .Returns(hashedPassword);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _userRepositoryMock.Verify(repo => repo.Add(It.IsAny<User>()), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_If_Email_Is_Not_Unique()
    {
        // Arrange
        var command = new CreateUserCommand(
            Email: "user@example.com",
            Password: "securePassword",
            FirstName: "John",
            LastName: "Doe",
            RoleName: "Admin");

        var email = Email.Create(command.Email).Value;

        _userRepositoryMock
            .Setup(repo => repo.IsEmailUniqueAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.User.EmailAlreadyInUse, result.Error);
        _userRepositoryMock.Verify(repo => repo.Add(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_RoleDoesNotExist()
    {
        // Arrange
        var command = new CreateUserCommand(
            Email: "user@example.com",
            Password: "securePassword",
            FirstName: "John",
            LastName: "Doe",
            RoleName: "NonExistentRole");

        var email = Email.Create(command.Email).Value;

        _userRepositoryMock
            .Setup(repo => repo.IsEmailUniqueAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _roleRepositoryMock
            .Setup(repo => repo.GetByNameAsync(command.RoleName, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Role)null!);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.User.InvalidRoleName, result.Error);
        _userRepositoryMock.Verify(repo => repo.Add(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_EmailIsInvalid()
    {
        // Arrange
        var command = new CreateUserCommand(
            Email: "invalid-email",
            Password: "securePassword",
            FirstName: "John",
            LastName: "Doe",
            RoleName: "Admin");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Email.InvalidFormat, result.Error);
        _userRepositoryMock.Verify(repo => repo.Add(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_FirstNameIsInvalid()
    {
        // Arrange
        var command = new CreateUserCommand(
            Email: "user@example.com",
            Password: "securePassword",
            FirstName: "",
            LastName: "Doe",
            RoleName: "Admin");
        
        var email = Email.Create(command.Email).Value;
        
        _userRepositoryMock
            .Setup(repo => repo.IsEmailUniqueAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.FirstName.Empty, result.Error);
        _userRepositoryMock.Verify(repo => repo.Add(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_LastNameIsInvalid()
    {
        // Arrange
        var command = new CreateUserCommand(
            Email: "user@example.com",
            Password: "securePassword",
            FirstName: "John",
            LastName: "",
            RoleName: "Admin");
        
        var email = Email.Create(command.Email).Value;
        
        _userRepositoryMock
            .Setup(repo => repo.IsEmailUniqueAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.LastName.Empty, result.Error);
        _userRepositoryMock.Verify(repo => repo.Add(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    #endregion
}
using InspireEd.Application.Faculties.Groups.Commands.AddStudentToGroup;
using InspireEd.Application.UnitTests.Common;
using InspireEd.Application.Users.Services;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Entities;
using Moq;

namespace InspireEd.Application.UnitTests.Faculties.Commands.Groups;

public class AddStudentToGroupCommandHandlerTests
{
    #region Fields & Mock Setup

    private readonly Mock<IUserCreationService> _userCreationServiceMock = new();
    private readonly Mock<IFacultyRepository> _facultyRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly AddStudentToGroupCommandHandler _handler;

    public AddStudentToGroupCommandHandlerTests()
    {
        _handler = new AddStudentToGroupCommandHandler(
            _userCreationServiceMock.Object,
            _facultyRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    #endregion

    #region Test Methods

    [Fact]
    public async Task Handle_Should_AddStudentToGroup_Successfully()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var command = new AddStudentToGroupCommand(
            facultyId,
            groupId,
            "John",
            "Doe",
            "john.doe@example.com",
            "password");

        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering Faculty");
        faculty.AddGroup(groupId, GroupName.Create("Group A").Value);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        _userCreationServiceMock
            .Setup(service => service.CreateUserAsync(
                command.StudentFirstName,
                command.StudentLastName,
                command.StudentEmail,
                command.StudentPassword,
                Role.Student.Name,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(studentId));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _userCreationServiceMock.Verify(service => service.CreateUserAsync(
            command.StudentFirstName,
            command.StudentLastName,
            command.StudentEmail,
            command.StudentPassword,
            Role.Student.Name,
            It.IsAny<CancellationToken>()), Times.Once);
        _facultyRepositoryMock.Verify(repo => repo.Update(faculty), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_FacultyNotFound()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var command = new AddStudentToGroupCommand(
            facultyId,
            groupId,
            "John",
            "Doe",
            "john.doe@example.com",
            "password");

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Faculty?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Faculty.NotFound(facultyId), result.Error);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _userCreationServiceMock.Verify(service => service.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _facultyRepositoryMock.Verify(repo => repo.Update(It.IsAny<Faculty>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_GroupNotFound()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var command = new AddStudentToGroupCommand(
            facultyId,
            groupId,
            "John",
            "Doe",
            "john.doe@example.com",
            "password");

        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering Faculty");

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Faculty.GroupDoesNotExist(groupId), result.Error);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _userCreationServiceMock.Verify(service => service.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _facultyRepositoryMock.Verify(repo => repo.Update(It.IsAny<Faculty>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_UserCreationFails()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var command = new AddStudentToGroupCommand(
            facultyId,
            groupId,
            "John",
            "Doe",
            "john.doe@example.com",
            "password");

        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering Faculty");
        faculty.AddGroup(groupId, GroupName.Create("Group A").Value);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        _userCreationServiceMock
            .Setup(service => service.CreateUserAsync(
                command.StudentFirstName,
                command.StudentLastName,
                command.StudentEmail,
                command.StudentPassword,
                Role.Student.Name,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<Guid>(DomainErrors.User.EmailAlreadyInUse));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.User.EmailAlreadyInUse, result.Error);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _userCreationServiceMock.Verify(service => service.CreateUserAsync(
            command.StudentFirstName,
            command.StudentLastName,
            command.StudentEmail,
            command.StudentPassword,
            Role.Student.Name,
            It.IsAny<CancellationToken>()), Times.Once);
        _facultyRepositoryMock.Verify(repo => repo.Update(It.IsAny<Faculty>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_DatabaseSaveFails()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var command = new AddStudentToGroupCommand(
            facultyId,
            groupId,
            "John",
            "Doe",
            "john.doe@example.com",
            "password");

        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering Faculty");
        faculty.AddGroup(groupId, GroupName.Create("Group A").Value);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        _userCreationServiceMock
            .Setup(service => service.CreateUserAsync(
                command.StudentFirstName,
                command.StudentLastName,
                command.StudentEmail,
                command.StudentPassword,
                Role.Student.Name,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(studentId));

        // Simulate a database failure
        _unitOfWorkMock
            .Setup(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await _handler.Handle(command, CancellationToken.None);
        });

        // Verify interactions
        _facultyRepositoryMock.Verify(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _userCreationServiceMock.Verify(service => service.CreateUserAsync(
            command.StudentFirstName,
            command.StudentLastName,
            command.StudentEmail,
            command.StudentPassword,
            Role.Student.Name,
            It.IsAny<CancellationToken>()), Times.Once);
        _facultyRepositoryMock.Verify(repo => repo.Update(faculty), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion
}
using InspireEd.Application.Faculties.Groups.Commands.RemoveStudentFromGroup;
using InspireEd.Application.UnitTests.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Users.Entities;
using InspireEd.Domain.Users.Repositories;
using Moq;

namespace InspireEd.Application.UnitTests.Faculties.Commands.Groups;

public class RemoveStudentFromGroupCommandHandlerTests
{
    #region Fields & Mock Setup

    private readonly Mock<IFacultyRepository> _facultyRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly RemoveStudentFromGroupCommandHandler _handler;

    public RemoveStudentFromGroupCommandHandlerTests()
    {
        _handler = new RemoveStudentFromGroupCommandHandler(
            _facultyRepositoryMock.Object,
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    #endregion

    #region Test Methods

    [Fact]
    public async Task Handle_Should_RemoveStudentFromGroup_Successfully()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var command = new RemoveStudentFromGroupCommand(facultyId, groupId, studentId);

        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering Faculty");
        var group = faculty.AddGroup(groupId, GroupName.Create("Group A").Value).Value;
        group.AddStudent(studentId);

        var student = Helpers.CreateTestUser(
            studentId,
            "john.doe@example.com",
            "hashedPassword",
            "John",
            "Doe",
            Role.Student.Name);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(studentId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Delete(student), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_FacultyNotFound()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var command = new RemoveStudentFromGroupCommand(facultyId, groupId, studentId);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Faculty?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Faculty.NotFound(facultyId), result.Error);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _userRepositoryMock.Verify(repo => repo.Delete(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_GroupNotFound()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var command = new RemoveStudentFromGroupCommand(facultyId, groupId, studentId);

        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering Faculty");

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Group.NotFound(groupId), result.Error);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _userRepositoryMock.Verify(repo => repo.Delete(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_StudentNotFoundInGroup()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var command = new RemoveStudentFromGroupCommand(facultyId, groupId, studentId);

        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering Faculty");
        faculty.AddGroup(groupId, GroupName.Create("Group A").Value);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Group.StudentNotExist(groupId, studentId), result.Error);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _userRepositoryMock.Verify(repo => repo.Delete(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_StudentNotFoundInDatabase()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var command = new RemoveStudentFromGroupCommand(facultyId, groupId, studentId);

        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering Faculty");
        var group = faculty.AddGroup(groupId, GroupName.Create("Group A").Value).Value;
        group.AddStudent(studentId);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.User.NotFound(studentId), result.Error);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(studentId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Delete(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_DatabaseSaveFails()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var command = new RemoveStudentFromGroupCommand(facultyId, groupId, studentId);

        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering Faculty");
        var group = faculty.AddGroup(groupId, GroupName.Create("Group A").Value).Value;
        group.AddStudent(studentId);

        var student = Helpers.CreateTestUser(
            studentId,
            "john.doe@example.com",
            "hashedPassword",
            "John",
            "Doe",
            Role.Student.Name);
        
        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

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
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(studentId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Delete(student), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion
}
using InspireEd.Application.Faculties.Groups.Commands.RemoveAllStudentsFromGroup;
using InspireEd.Application.UnitTests.Faculties.Commands.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Users.Entities;
using InspireEd.Domain.Users.Repositories;
using Moq;

namespace InspireEd.Application.UnitTests.Faculties.Commands.Groups;

public class RemoveAllStudentsFromGroupCommandHandlerTests
{
    #region Fields & Mock Setup

    private readonly Mock<IFacultyRepository> _facultyRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly RemoveAllStudentsFromGroupCommandHandler _handler;

    public RemoveAllStudentsFromGroupCommandHandlerTests()
    {
        _handler = new RemoveAllStudentsFromGroupCommandHandler(
            _facultyRepositoryMock.Object,
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    #endregion

    #region Test Methods

    [Fact]
    public async Task Handle_Should_RemoveAllStudentsFromGroup_Successfully()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var studentId1 = Guid.NewGuid();
        var studentId2 = Guid.NewGuid();
        var command = new RemoveAllStudentsFromGroupCommand(facultyId, groupId);

        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering Faculty");
        var group = faculty.AddGroup(groupId, GroupName.Create("Group A").Value).Value;
        group.AddStudent(studentId1);
        group.AddStudent(studentId2);

        var student1 = Helpers.CreateTestUser(
            studentId1,
            "john.doe@example.com",
            "hashedPassword",
            "John",
            "Doe",
            Role.Student.Name);
        var student2 = Helpers.CreateTestUser(
            studentId2,
            "jane.doe@example.com",
            "hashedPassword",
            "Jane",
            "Doe",
            Role.Student.Name);
        
        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([student1, student2]);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Delete(student1), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Delete(student2), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_FacultyNotFound()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var command = new RemoveAllStudentsFromGroupCommand(facultyId, groupId);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Faculty?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Faculty.NotFound(facultyId), result.Error);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()), Times.Never);
        _userRepositoryMock.Verify(repo => repo.Delete(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_GroupNotFound()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var command = new RemoveAllStudentsFromGroupCommand(facultyId, groupId);

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
        _userRepositoryMock.Verify(repo => repo.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()), Times.Never);
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
        var command = new RemoveAllStudentsFromGroupCommand(facultyId, groupId);

        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering Faculty");
        var group = faculty.AddGroup(groupId, GroupName.Create("Group A").Value).Value;
        group.AddStudent(studentId);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.User.NotFound(studentId), result.Error);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Delete(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_DatabaseSaveFails()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var studentId1 = Guid.NewGuid();
        var studentId2 = Guid.NewGuid();
        var command = new RemoveAllStudentsFromGroupCommand(facultyId, groupId);

        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering Faculty");
        var group = faculty.AddGroup(groupId, GroupName.Create("Group A").Value).Value;
        group.AddStudent(studentId1);
        group.AddStudent(studentId2);

        var student1 = Helpers.CreateTestUser(
            studentId1,
            "john.doe@example.com",
            "hashedPassword",
            "John",
            "Doe",
            Role.Student.Name);
        
        
        var student2 = Helpers.CreateTestUser(
            studentId2,
            "jane.doe@example.com",
            "hashedPassword",
            "John",
            "Doe",
            Role.Student.Name);
        
        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([student1, student2]);

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
        _userRepositoryMock.Verify(repo => repo.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Delete(student1), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Delete(student2), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion
}
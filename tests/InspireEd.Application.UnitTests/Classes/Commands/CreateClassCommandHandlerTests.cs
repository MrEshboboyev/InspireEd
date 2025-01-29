using InspireEd.Application.Classes.Commands.CreateClass;
using InspireEd.Application.UnitTests.Common;
using InspireEd.Domain.Classes.Entities;
using InspireEd.Domain.Classes.Enums;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Subjects.Entities;
using InspireEd.Domain.Subjects.Repositories;
using InspireEd.Domain.Users.Entities;
using InspireEd.Domain.Users.Repositories;
using Moq;

namespace InspireEd.Application.UnitTests.Classes.Commands;

public class CreateClassCommandHandlerTests
{
    #region Fields & Mock Setup

    private readonly Mock<ISubjectRepository> _subjectRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IGroupRepository> _groupRepositoryMock = new();
    private readonly Mock<IClassRepository> _classRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly CreateClassCommandHandler _handler;

    public CreateClassCommandHandlerTests()
    {
        _handler = new CreateClassCommandHandler(
            _subjectRepositoryMock.Object,
            _userRepositoryMock.Object,
            _groupRepositoryMock.Object,
            _classRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    #endregion

    #region Test Methods

    [Fact]
    public async Task Handle_Should_CreateClass_Successfully()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        var facultyId = Guid.NewGuid();
        var groupIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var scheduledDate = DateTime.UtcNow.AddDays(1);
        var command = new CreateClassCommand(
            subjectId,
            teacherId,
            ClassType.Lecture,
            groupIds,
            scheduledDate);

        var subject = Helpers.CreateTestSubject(
            subjectId,
            "Math",
            "MATH101",
            3);
        var teacher = Helpers.CreateTestUser(
            teacherId,
            "teacher@example.com",
            "password-hash",
            "John",
            "Doe",
            "Teacher");

        var faculty = Helpers.CreateTestFaculty(
            facultyId,
            "faculty-name");
        
        List<Group> groups = [
            faculty.AddGroup(
                    groupIds[0],
                    GroupName.Create("Group A").Value)
                .Value,
            faculty.AddGroup(
                    groupIds[1],
                    GroupName.Create("Group B").Value)
                .Value];

        _subjectRepositoryMock
            .Setup(repo => repo.GetByIdAsync(subjectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subject);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(teacherId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(teacher);

        _groupRepositoryMock
            .Setup(repo => repo.GetByIdsAsync(groupIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync(groups);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _subjectRepositoryMock.Verify(repo => repo.GetByIdAsync(subjectId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(teacherId, It.IsAny<CancellationToken>()), Times.Once);
        _groupRepositoryMock.Verify(repo => repo.GetByIdsAsync(groupIds, It.IsAny<CancellationToken>()), Times.Once);
        _classRepositoryMock.Verify(repo => repo.Add(It.IsAny<Class>()), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_SubjectNotFound()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        var groupIds = new List<Guid> { Guid.NewGuid() };
        var scheduledDate = DateTime.UtcNow.AddDays(1);
        var command = new CreateClassCommand(
            subjectId,
            teacherId,
            ClassType.Lecture,
            groupIds,
            scheduledDate);

        _subjectRepositoryMock
            .Setup(repo => repo.GetByIdAsync(subjectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Subject?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Subject.NotFound(subjectId), result.Error);
        _subjectRepositoryMock.Verify(repo => repo.GetByIdAsync(subjectId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _groupRepositoryMock.Verify(repo => repo.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _classRepositoryMock.Verify(repo => repo.Add(It.IsAny<Class>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_TeacherNotFound()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        var groupIds = new List<Guid> { Guid.NewGuid() };
        var scheduledDate = DateTime.UtcNow.AddDays(1);
        var command = new CreateClassCommand(
            subjectId,
            teacherId,
            ClassType.Lecture,
            groupIds,
            scheduledDate);

        var subject = Helpers.CreateTestSubject(
            subjectId,
            "Math",
            "MATH101",
            3);

        _subjectRepositoryMock
            .Setup(repo => repo.GetByIdAsync(subjectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subject);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(teacherId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Teacher.NotFound(teacherId), result.Error);
        _subjectRepositoryMock.Verify(repo => repo.GetByIdAsync(subjectId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(teacherId, It.IsAny<CancellationToken>()), Times.Once);
        _groupRepositoryMock.Verify(repo => repo.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _classRepositoryMock.Verify(repo => repo.Add(It.IsAny<Class>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_GroupNotFound()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        var facultyId = Guid.NewGuid();
        var groupIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var scheduledDate = DateTime.UtcNow.AddDays(1);
        var command = new CreateClassCommand(
            subjectId,
            teacherId,
            ClassType.Lecture,
            groupIds,
            scheduledDate);

        var subject = Helpers.CreateTestSubject(
            subjectId,
            "Math",
            "MATH101",
            3);
        var teacher = Helpers.CreateTestUser(
            teacherId,
            "teacher@example.com",
            "password-hash",
            "John",
            "Doe",
            "Teacher");
        var faculty = Helpers.CreateTestFaculty(
            facultyId,
            "faculty-name");
        var groups = new List<Group>
        {
            faculty.AddGroup(
                groupIds[0],
                GroupName.Create("Group A").Value).Value
        }; // Only one group exists

        _subjectRepositoryMock
            .Setup(repo => repo.GetByIdAsync(subjectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subject);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(teacherId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(teacher);

        _groupRepositoryMock
            .Setup(repo => repo.GetByIdsAsync(groupIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync(groups);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Group.MissingGroups(new List<Guid> { groupIds[1] }), result.Error);
        _subjectRepositoryMock.Verify(repo => repo.GetByIdAsync(subjectId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(teacherId, It.IsAny<CancellationToken>()), Times.Once);
        _groupRepositoryMock.Verify(repo => repo.GetByIdsAsync(groupIds, It.IsAny<CancellationToken>()), Times.Once);
        _classRepositoryMock.Verify(repo => repo.Add(It.IsAny<Class>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    #endregion
}
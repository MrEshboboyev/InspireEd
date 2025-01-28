using InspireEd.Application.Faculties.Commands.RemoveGroupFromFaculty;
using InspireEd.Application.UnitTests.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Repositories;
using Moq;

namespace InspireEd.Application.UnitTests.Faculties.Commands;

public class RemoveGroupFromFacultyCommandHandlerTests
{
    #region Fields & Mock Setup

    private readonly Mock<IFacultyRepository> _facultyRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly RemoveGroupFromFacultyCommandHandler _handler;

    public RemoveGroupFromFacultyCommandHandlerTests()
    {
        _handler = new RemoveGroupFromFacultyCommandHandler(
            _facultyRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    #endregion

    #region Test Methods

    [Fact]
    public async Task Handle_Should_RemoveGroupFromFaculty_Successfully()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var command = new RemoveGroupFromFacultyCommand(facultyId, groupId);

        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering Faculty");
        faculty.AddGroup(groupId, GroupName.Create("Group A").Value);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _facultyRepositoryMock.Verify(repo => repo.Update(faculty), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_FacultyNotFound()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var command = new RemoveGroupFromFacultyCommand(facultyId, groupId);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Faculty?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Faculty.NotFound(facultyId), result.Error);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _facultyRepositoryMock.Verify(repo => repo.Update(It.IsAny<Faculty>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_GroupNotFound()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var command = new RemoveGroupFromFacultyCommand(facultyId, groupId);

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
        _facultyRepositoryMock.Verify(repo => repo.Update(It.IsAny<Faculty>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_DatabaseSaveFails()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var command = new RemoveGroupFromFacultyCommand(facultyId, groupId);

        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering Faculty");
        faculty.AddGroup(groupId, GroupName.Create("Group A").Value);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

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
        _facultyRepositoryMock.Verify(repo => repo.Update(faculty), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion
}
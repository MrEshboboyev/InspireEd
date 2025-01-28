using InspireEd.Application.Faculties.Commands.AddGroupToFaculty;
using InspireEd.Application.UnitTests.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Repositories;
using Moq;

namespace InspireEd.Application.UnitTests.Faculties.Commands;

public class AddGroupToFacultyCommandHandlerTests
{
    #region Fields & Mock Setup

    private readonly Mock<IFacultyRepository> _facultyRepositoryMock = new();
    private readonly Mock<IGroupRepository> _groupRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly AddGroupToFacultyCommandHandler _handler;

    public AddGroupToFacultyCommandHandlerTests()
    {
        _handler = new AddGroupToFacultyCommandHandler(
            _facultyRepositoryMock.Object,
            _groupRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    #endregion

    #region Test Methods

    [Fact]
    public async Task Handle_Should_AddGroupToFaculty_Successfully()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupName = "Group A";
        var command = new AddGroupToFacultyCommand(facultyId, groupName);

        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering Faculty");

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        _groupRepositoryMock
            .Setup(repo => repo.Add(It.IsAny<Group>()));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _groupRepositoryMock.Verify(repo => repo.Add(It.IsAny<Group>()), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_FacultyNotFound()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupName = "Group A";
        var command = new AddGroupToFacultyCommand(facultyId, groupName);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Faculty?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Faculty.NotFound(facultyId), result.Error);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _groupRepositoryMock.Verify(repo => repo.Add(It.IsAny<Group>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_GroupNameIsInvalid()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupName = ""; // Invalid group name
        var command = new AddGroupToFacultyCommand(facultyId, groupName);

        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering Faculty");

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.GroupName.Empty, result.Error);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _groupRepositoryMock.Verify(repo => repo.Add(It.IsAny<Group>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_DatabaseSaveFails()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var groupName = "Group A";
        var command = new AddGroupToFacultyCommand(facultyId, groupName);

        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering Faculty");

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        _groupRepositoryMock
            .Setup(repo => repo.Add(It.IsAny<Group>()));

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
        _groupRepositoryMock.Verify(repo => repo.Add(It.IsAny<Group>()), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion
}
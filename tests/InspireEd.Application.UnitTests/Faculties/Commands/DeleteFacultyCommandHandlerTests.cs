using InspireEd.Application.Faculties.Commands.DeleteFaculty;
using InspireEd.Application.UnitTests.Faculties.Commands.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Repositories;
using Moq;

namespace InspireEd.Application.UnitTests.Faculties.Commands;

public class DeleteFacultyCommandHandlerTests
{
    #region Fields & Mock Setup
    
    private readonly Mock<IFacultyRepository> _facultyRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly DeleteFacultyCommandHandler _handler;

    public DeleteFacultyCommandHandlerTests()
    {
        _handler = new DeleteFacultyCommandHandler(
            _facultyRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }
    
    #endregion

    #region Test Methods
    
    [Fact]
    public async Task Handle_Should_DeleteFaculty_Successfully()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var command = new DeleteFacultyCommand(facultyId);

        var faculty = Helpers.CreateTestFaculty(facultyId, "test-faculty");

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _facultyRepositoryMock.Verify(repo => repo.Remove(faculty), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_FacultyNotFound()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var command = new DeleteFacultyCommand(facultyId);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Faculty)null!);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Faculty.NotFound(facultyId), result.Error);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _facultyRepositoryMock.Verify(repo => repo.Remove(It.IsAny<Faculty>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_SaveChangesThrowsException()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var command = new DeleteFacultyCommand(facultyId);

        var faculty = Helpers.CreateTestFaculty(facultyId, "test-faculty");

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        _unitOfWorkMock
            .Setup(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

        _facultyRepositoryMock.Verify(repo => repo.Remove(faculty), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    #endregion
}
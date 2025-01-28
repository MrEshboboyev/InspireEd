using InspireEd.Application.Faculties.Commands.RenameFaculty;
using InspireEd.Application.UnitTests.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Repositories;
using Moq;

namespace InspireEd.Application.UnitTests.Faculties.Commands;

public class RenameFacultyCommandHandlerTests
{
    #region Fields & Mock Setup
    
    private readonly Mock<IFacultyRepository> _facultyRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly RenameFacultyCommandHandler _handler;

    public RenameFacultyCommandHandlerTests()
    {
        _handler = new RenameFacultyCommandHandler(
            _facultyRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }
    
    #endregion
    
    #region Test Methods

    [Fact]
    public async Task Handle_Should_RenameFaculty_Successfully()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var newFacultyName = "Updated Faculty Name";
        var newFacultyNameObj = FacultyName.Create(newFacultyName).Value;
        var command = new RenameFacultyCommand(facultyId, newFacultyName);

        var faculty = Helpers.CreateTestFaculty(facultyId, "Original Faculty Name");

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newFacultyNameObj, faculty.Name);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _facultyRepositoryMock.Verify(repo => repo.Update(faculty), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_FacultyNotFound()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var newFacultyName = "Updated Faculty Name";
        var command = new RenameFacultyCommand(facultyId, newFacultyName);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Faculty)null!);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Faculty.NotFound(facultyId), result.Error);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _facultyRepositoryMock.Verify(repo => repo.Update(It.IsAny<Faculty>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_FacultyNameIsInvalid()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var invalidFacultyName = ""; // Invalid name
        var command = new RenameFacultyCommand(facultyId, invalidFacultyName);

        var faculty = Helpers.CreateTestFaculty(facultyId, "Original Faculty Name");

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.FacultyName.Empty, result.Error);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _facultyRepositoryMock.Verify(repo => repo.Update(It.IsAny<Faculty>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_UpdateNameFails()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var newFacultyName = "";
        var command = new RenameFacultyCommand(facultyId, newFacultyName);

        var faculty = Helpers.CreateTestFaculty(facultyId, "Original Faculty Name");

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.FacultyName.Empty, result.Error);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _facultyRepositoryMock.Verify(repo => repo.Update(It.IsAny<Faculty>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_SaveChangesThrowsException()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var newFacultyName = "Updated Faculty Name";
        var command = new RenameFacultyCommand(facultyId, newFacultyName);

        var faculty = Helpers.CreateTestFaculty(facultyId, "Original Faculty Name");

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        _unitOfWorkMock
            .Setup(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

        _facultyRepositoryMock.Verify(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _facultyRepositoryMock.Verify(repo => repo.Update(faculty), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    #endregion
}
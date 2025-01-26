using InspireEd.Application.Faculties.Commands.CreateFaculty;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Repositories;
using Moq;

namespace InspireEd.Application.UnitTests.Faculties.Commands;

public class CreateFacultyCommandHandlerTests
{
    #region Fields & Mock Setup
    
    private readonly Mock<IFacultyRepository> _facultyRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly CreateFacultyCommandHandler _handler;

    public CreateFacultyCommandHandlerTests()
    {
        _handler = new CreateFacultyCommandHandler(
            _facultyRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }
    
    #endregion

    #region Test Methods
    
    [Fact]
    public async Task Handle_Should_CreateFaculty_Successfully()
    {
        // Arrange
        var facultyName = "Engineering Faculty";
        var command = new CreateFacultyCommand(facultyName);

        _facultyRepositoryMock
            .Setup(repo => repo.Add(It.IsAny<Faculty>()));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _facultyRepositoryMock.Verify(repo => repo.Add(It.IsAny<Faculty>()), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_FacultyNameIsInvalid()
    {
        // Arrange
        var facultyName = ""; // Invalid name
        var command = new CreateFacultyCommand(facultyName);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.FacultyName.Empty, result.Error);
        _facultyRepositoryMock.Verify(repo => repo.Add(It.IsAny<Faculty>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_SaveChangesThrowsException()
    {
        // Arrange
        var facultyName = "Engineering Faculty";
        var command = new CreateFacultyCommand(facultyName);

        _facultyRepositoryMock
            .Setup(repo => repo.Add(It.IsAny<Faculty>()));

        _unitOfWorkMock
            .Setup(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

        _facultyRepositoryMock.Verify(repo => repo.Add(It.IsAny<Faculty>()), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    #endregion
}
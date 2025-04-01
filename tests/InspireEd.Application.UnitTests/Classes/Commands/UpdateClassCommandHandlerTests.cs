using InspireEd.Application.Classes.Commands.UpdateClass;
using InspireEd.Application.UnitTests.Common;
using InspireEd.Domain.Classes.Entities;
using InspireEd.Domain.Classes.Enums;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using Moq;

namespace InspireEd.Application.UnitTests.Classes.Commands;

public class UpdateClassCommandHandlerTests
{
    #region Fields & Mock Setup

    private readonly Mock<IClassRepository> _classRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly UpdateClassCommandHandler _handler;

    public UpdateClassCommandHandlerTests()
    {
        _handler = new UpdateClassCommandHandler(
            _classRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    #endregion

    #region Test Methods

    [Fact]
    public async Task Handle_Should_UpdateClass_Successfully()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        var scheduledDate = DateTime.UtcNow.AddDays(1);
        var command = new UpdateClassCommand(
            classId,
            subjectId,
            teacherId,
            ClassType.Lecture,
            scheduledDate);

        var classEntity = Helpers.CreateTestClass(
            classId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            ClassType.Laboratory,
            [],
            DateTime.UtcNow);

        _classRepositoryMock
            .Setup(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(classEntity);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _classRepositoryMock.Verify(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()), Times.Once);
        _classRepositoryMock.Verify(repo => repo.Update(classEntity), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_ClassNotFound()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var command = new UpdateClassCommand(
            classId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            ClassType.Lecture,
            DateTime.UtcNow.AddDays(1));

        _classRepositoryMock
            .Setup(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Class)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Class.NotFound(classId), result.Error);
        _classRepositoryMock.Verify(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()), Times.Once);
        _classRepositoryMock.Verify(repo => repo.Update(It.IsAny<Class>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_UpdateClassDetailsFails()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var command = new UpdateClassCommand(
            classId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            ClassType.Lecture,
            DateTime.UtcNow.AddDays(-1)); // Invalid scheduled date (in the past)

        var classEntity = Helpers.CreateTestClass(
            classId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            ClassType.Laboratory,
            [],
            DateTime.UtcNow);

        _classRepositoryMock
            .Setup(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(classEntity);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        _classRepositoryMock.Verify(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()), Times.Once);
        _classRepositoryMock.Verify(repo => repo.Update(It.IsAny<Class>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    #endregion
}
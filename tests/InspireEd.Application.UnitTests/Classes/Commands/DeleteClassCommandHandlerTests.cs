using InspireEd.Application.Classes.Commands.DeleteClass;
using InspireEd.Application.UnitTests.Common;
using InspireEd.Domain.Classes.Entities;
using InspireEd.Domain.Classes.Enums;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using Moq;

namespace InspireEd.Application.UnitTests.Classes.Commands;

public class DeleteClassCommandHandlerTests
{
    #region Fields & Mock Setup

    private readonly Mock<IClassRepository> _classRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly DeleteClassCommandHandler _handler;

    public DeleteClassCommandHandlerTests()
    {
        _handler = new DeleteClassCommandHandler(
            _classRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    #endregion

    #region Test Methods

    [Fact]
    public async Task Handle_Should_DeleteClass_Successfully()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var command = new DeleteClassCommand(classId);

        var classEntity = Helpers.CreateTestClass(
            classId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            ClassType.Lecture,
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
        _classRepositoryMock.Verify(repo => repo.Remove(classEntity), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_ClassNotFound()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var command = new DeleteClassCommand(classId);

        _classRepositoryMock
            .Setup(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Class)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Class.NotFound(classId), result.Error);
        _classRepositoryMock.Verify(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()), Times.Once);
        _classRepositoryMock.Verify(repo => repo.Remove(It.IsAny<Class>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    #endregion
}
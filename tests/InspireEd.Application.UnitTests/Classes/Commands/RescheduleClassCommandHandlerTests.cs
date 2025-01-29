using Moq;
using InspireEd.Application.Classes.Commands.RescheduleClass;
using InspireEd.Application.UnitTests.Common;
using InspireEd.Domain.Classes.Entities;
using InspireEd.Domain.Classes.Enums;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;

namespace InspireEd.Application.UnitTests.Classes.Commands;

public class RescheduleClassCommandHandlerTests
{
    private readonly Mock<IClassRepository> _classRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly RescheduleClassCommandHandler _handler;

    public RescheduleClassCommandHandlerTests()
    {
        _classRepositoryMock = new Mock<IClassRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new RescheduleClassCommandHandler(
            _classRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenClassExistsAndCanBeRescheduled()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var newScheduledDate = DateTime.UtcNow.AddDays(5);
        var classEntity = Helpers.CreateTestClass(
            classId, 
            Guid.NewGuid(),
            Guid.NewGuid(),
            ClassType.Laboratory,
            [],
            DateTime.UtcNow.AddDays(1));

        _classRepositoryMock.Setup(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(classEntity);

        // Act
        var result =
            await _handler.Handle(new RescheduleClassCommand(classId, newScheduledDate), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _classRepositoryMock.Verify(repo => repo.Update(classEntity), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenClassDoesNotExist()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var newScheduledDate = DateTime.UtcNow.AddDays(5);

        _classRepositoryMock.Setup(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Class)null!);

        // Act
        var result =
            await _handler.Handle(new RescheduleClassCommand(classId, newScheduledDate), CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Class.NotFound(classId), result.Error);
        _classRepositoryMock.Verify(repo => repo.Update(It.IsAny<Class>()), Times.Never);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRescheduleFails()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var newScheduledDate = DateTime.UtcNow.AddDays(-1);
        var classEntity = Helpers.CreateTestClass(
            classId, 
            Guid.NewGuid(),
            Guid.NewGuid(),
            ClassType.Laboratory,
            [],
            DateTime.UtcNow.AddDays(1));

        _classRepositoryMock.Setup(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(classEntity);

        // Force failure
        classEntity.Reschedule(newScheduledDate); // Simulate failure scenario.

        // Act
        var result =
            await _handler.Handle(new RescheduleClassCommand(classId, newScheduledDate), CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Class.InvalidScheduledDate, result.Error);
        _classRepositoryMock.Verify(repo => repo.Update(It.IsAny<Class>()), Times.Never);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
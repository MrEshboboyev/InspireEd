using InspireEd.Application.Classes.Attendances.Commands.DeleteAttendance;
using InspireEd.Application.UnitTests.Common;
using InspireEd.Domain.Classes.Entities;
using InspireEd.Domain.Classes.Enums;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using Moq;

namespace InspireEd.Application.UnitTests.Classes.Attendances.Commands;

public class DeleteAttendanceCommandHandlerTests
{
    #region Fields & Mock Setup

    private readonly Mock<IClassRepository> _classRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly DeleteAttendanceCommandHandler _handler;

    public DeleteAttendanceCommandHandlerTests()
    {
        _handler = new DeleteAttendanceCommandHandler(
            _classRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    #endregion

    #region Test Methods

    [Fact]
    public async Task Handle_Should_DeleteAttendance_Successfully()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        
        var classEntity = Helpers.CreateTestClass(
            classId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            ClassType.Lecture,
            [],
            DateTime.UtcNow);

        var attendance = classEntity.AddAttendance(
            studentId,
            AttendanceStatus.Present,
            "Notes").Value;

        
        var command = new DeleteAttendanceCommand(classId, attendance.Id);

        _classRepositoryMock
            .Setup(repo => repo.GetByIdWithAttendancesAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(classEntity);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _classRepositoryMock.Verify(repo => repo.GetByIdWithAttendancesAsync(classId, It.IsAny<CancellationToken>()), Times.Once);
        _classRepositoryMock.Verify(repo => repo.Update(classEntity), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_ClassNotFound()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var attendanceId = Guid.NewGuid();
        var command = new DeleteAttendanceCommand(classId, attendanceId);

        _classRepositoryMock
            .Setup(repo => repo.GetByIdWithAttendancesAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Class)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Class.NotFound(classId), result.Error);
        _classRepositoryMock.Verify(repo => repo.GetByIdWithAttendancesAsync(classId, It.IsAny<CancellationToken>()), Times.Once);
        _classRepositoryMock.Verify(repo => repo.Update(It.IsAny<Class>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_AttendanceNotFound()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var attendanceId = Guid.NewGuid();
        var command = new DeleteAttendanceCommand(classId, attendanceId);

        var classEntity = Helpers.CreateTestClass(
            classId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            ClassType.Lecture,
            [],
            DateTime.UtcNow);

        _classRepositoryMock
            .Setup(repo => repo.GetByIdWithAttendancesAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(classEntity);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Attendance.NotFound(attendanceId), result.Error);
        _classRepositoryMock.Verify(repo => repo.GetByIdWithAttendancesAsync(classId, It.IsAny<CancellationToken>()), Times.Once);
        _classRepositoryMock.Verify(repo => repo.Update(It.IsAny<Class>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_RemoveAttendanceFails()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        
        var classEntity = Helpers.CreateTestClass(
            classId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            ClassType.Lecture,
            [],
            DateTime.UtcNow);

        var attendance = classEntity.AddAttendance(
            studentId,
            AttendanceStatus.Present,
            "Notes").Value;
        
        var command = new DeleteAttendanceCommand(classId, attendance.Id);

        _classRepositoryMock
            .Setup(repo => repo.GetByIdWithAttendancesAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(classEntity);

        // Simulate a failure in RemoveAttendance
        classEntity.RemoveAttendance(attendance); // Assume this fails internally

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        _classRepositoryMock.Verify(repo => repo.GetByIdWithAttendancesAsync(classId, It.IsAny<CancellationToken>()), Times.Once);
        _classRepositoryMock.Verify(repo => repo.Update(classEntity), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    #endregion
}
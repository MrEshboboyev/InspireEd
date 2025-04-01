using FluentAssertions;
using InspireEd.Application.Classes.Attendances.Commands.UpdateAttendance;
using InspireEd.Application.UnitTests.Common;
using InspireEd.Domain.Classes.Entities;
using InspireEd.Domain.Classes.Enums;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using Moq;

namespace InspireEd.Application.UnitTests.Classes.Attendances.Commands;

public class UpdateAttendanceCommandHandlerTests
{
    #region Fields & Mock Setup

    private readonly Mock<IClassRepository> _classRepositoryMock = new();
    private readonly Mock<IAttendanceRepository> _attendanceRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly UpdateAttendanceCommandHandler _handler;

    public UpdateAttendanceCommandHandlerTests()
    {
        _handler = new UpdateAttendanceCommandHandler(
            _classRepositoryMock.Object,
            _attendanceRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    #endregion

    #region Test Methods

    [Fact]
    public async Task Handle_Should_UpdateAttendance_Successfully()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var attendanceStatus = AttendanceStatus.Absent;
        var notes = "Arrived 10 minutes late";
        
        
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
            "On time").Value;

        var command = new UpdateAttendanceCommand(classId, attendance.Id, attendanceStatus, notes);


        _classRepositoryMock
            .Setup(repo => repo.GetByIdWithAttendancesAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(classEntity);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _attendanceRepositoryMock.Verify(repo => repo.Update(attendance), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_ClassNotFound()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var attendanceId = Guid.NewGuid();
        var command = new UpdateAttendanceCommand(classId, attendanceId, AttendanceStatus.Present, "Updated Notes");

        _classRepositoryMock
            .Setup(repo => repo.GetByIdWithAttendancesAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Class)null); // Simulate class not found

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Class.NotFound(classId));
        _attendanceRepositoryMock.Verify(repo => repo.Update(It.IsAny<Attendance>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_AttendanceNotFound()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var attendanceId = Guid.NewGuid();
        var command =
            new UpdateAttendanceCommand(classId, attendanceId, AttendanceStatus.Absent, "Absent without notice");

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
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Attendance.NotFound(attendanceId));
        _attendanceRepositoryMock.Verify(repo => repo.Update(It.IsAny<Attendance>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_UpdateAttendanceFails()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var attendanceId = Guid.NewGuid();
        var attendanceStatus = AttendanceStatus.Present;
        var notes = "Updated Notes";

        var command = new UpdateAttendanceCommand(classId, attendanceId, attendanceStatus, notes);

        var classEntity = Helpers.CreateTestClass(
            classId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            ClassType.Lecture,
            [],
            DateTime.UtcNow);

        classEntity.AddAttendance(
            attendanceId,
            AttendanceStatus.Absent,
            "Previous Notes");

        _classRepositoryMock
            .Setup(repo => repo.GetByIdWithAttendancesAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(classEntity);

        // Simulate update failure
        classEntity.UpdateAttendance(attendanceId, attendanceStatus, notes);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        _attendanceRepositoryMock.Verify(repo => repo.Update(It.IsAny<Attendance>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    #endregion
}
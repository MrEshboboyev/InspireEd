using FluentAssertions;
using InspireEd.Application.Classes.Commands.CreateAttendances;
using InspireEd.Application.UnitTests.Common;
using InspireEd.Domain.Classes.Entities;
using InspireEd.Domain.Classes.Enums;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Repositories;
using Moq;

namespace InspireEd.Application.UnitTests.Classes.Commands;

public class CreateAttendancesCommandHandlerTests
{
    #region Fields & Mock Setup
    
    private readonly Mock<IClassRepository> _classRepositoryMock;
    private readonly Mock<IAttendanceRepository> _attendanceRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGroupRepository> _groupRepositoryMock;
    private readonly CreateAttendancesCommandHandler _handler;

    public CreateAttendancesCommandHandlerTests()
    {
        _classRepositoryMock = new Mock<IClassRepository>();
        _attendanceRepositoryMock = new Mock<IAttendanceRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _groupRepositoryMock = new Mock<IGroupRepository>();

        _handler = new CreateAttendancesCommandHandler(
            _classRepositoryMock.Object,
            _attendanceRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _groupRepositoryMock.Object);
    }
    
    #endregion

    #region Test Methods
    
    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAttendanceIsCreated()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var attendanceStatus = AttendanceStatus.Present;
        var notes = "On time";

        var attendances = new List<(Guid StudentId, AttendanceStatus Status, string Notes)>
        {
            (studentId, attendanceStatus, notes)
        };

        var command = new CreateAttendancesCommand(classId, teacherId, attendances);

        // **Use real instance of Class**
        var classEntity = Helpers.CreateTestClass(
            classId,
            subjectId,
            teacherId,
            ClassType.Lecture,
            [Guid.NewGuid()],
            DateTime.UtcNow.AddDays(1));

        _classRepositoryMock.Setup(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(classEntity);

        _groupRepositoryMock.Setup(repo =>
                repo.GetStudentIdsForGroupsAsync(classEntity.GroupIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync([studentId]);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _attendanceRepositoryMock.Verify(repo => repo.Add(It.IsAny<Attendance>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenClassNotFound()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        var attendances = new List<(Guid StudentId, AttendanceStatus Status, string Notes)>();

        var command = new CreateAttendancesCommand(classId, teacherId, attendances);

        _classRepositoryMock.Setup(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Class)null!); // Simulate not found

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Class.NotFound(classId));
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenTeacherNotAssignedToClass()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        var wrongTeacherId = Guid.NewGuid();
        var attendances = new List<(Guid StudentId, AttendanceStatus Status, string Notes)>();

        var command = new CreateAttendancesCommand(classId, wrongTeacherId, attendances);

        var classEntity = Helpers.CreateTestClass(
            classId,
            subjectId,
            teacherId,
            ClassType.Lecture,
            [Guid.NewGuid()],
            DateTime.UtcNow.AddDays(1));

        _classRepositoryMock.Setup(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(classEntity);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Teacher.NotAssignedToClass(wrongTeacherId, classId));
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenStudentNotInClassGroup()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        var studentId = Guid.NewGuid(); // Student not in group

        var attendances = new List<(Guid StudentId, AttendanceStatus Status, string Notes)>
        {
            (studentId, AttendanceStatus.Present, "On time")
        };

        var command = new CreateAttendancesCommand(classId, teacherId, attendances);

        var classEntity = Helpers.CreateTestClass(
            classId,
            subjectId,
            teacherId,
            ClassType.Lecture,
            [Guid.NewGuid()],
            DateTime.UtcNow.AddDays(1));

        _classRepositoryMock.Setup(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(classEntity);

        _groupRepositoryMock.Setup(repo =>
                repo.GetStudentIdsForGroupsAsync(classEntity.GroupIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]); // No students found in group

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Class.StudentNotExist(classId, [studentId]));
    }

    [Fact]
    public async Task Handle_ShouldSaveAttendanceToRepository()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var attendanceStatus = AttendanceStatus.Present;
        var notes = "On time";

        var attendances = new List<(Guid StudentId, AttendanceStatus Status, string Notes)>
        {
            (studentId, attendanceStatus, notes)
        };

        var command = new CreateAttendancesCommand(classId, teacherId, attendances);

        var classEntity = Helpers.CreateTestClass(
            classId,
            subjectId,
            teacherId,
            ClassType.Lecture,
            [Guid.NewGuid()],
            DateTime.UtcNow.AddDays(1));
        
        _classRepositoryMock.Setup(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(classEntity);

        _groupRepositoryMock.Setup(repo =>
                repo.GetStudentIdsForGroupsAsync(classEntity.GroupIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync([studentId]);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _attendanceRepositoryMock.Verify(repo => repo.Add(It.IsAny<Attendance>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    #endregion
}
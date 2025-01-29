using Moq;
using InspireEd.Application.Classes.Commands.ChangeClassTeacher;
using InspireEd.Application.UnitTests.Common;
using InspireEd.Domain.Classes.Entities;
using InspireEd.Domain.Classes.Enums;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Users.Entities;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.UnitTests.Classes.Commands;

public class ChangeClassTeacherCommandHandlerTests
{
    #region Fields & Mock Setup
    
    private readonly Mock<IClassRepository> _classRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ChangeClassTeacherCommandHandler _handler;

    public ChangeClassTeacherCommandHandlerTests()
    {
        _classRepositoryMock = new Mock<IClassRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new ChangeClassTeacherCommandHandler(
            _classRepositoryMock.Object,
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }
    
    #endregion
    
    #region Test Methods

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenClassAndTeacherExistAndCanBeChanged()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        var classEntity = Helpers.CreateTestClass(
            classId, 
            Guid.NewGuid(),
            teacherId,
            ClassType.Laboratory,
            [],
            DateTime.UtcNow.AddDays(1));
        var teacher = Helpers.CreateTestUser(
            teacherId,
            "teacher@example.com",
            "password-hash",
            "John",
            "Doe",
            "Teacher");

        _classRepositoryMock.Setup(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(classEntity);
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(teacherId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(teacher);

        // Act
        var result = await _handler.Handle(new ChangeClassTeacherCommand(classId, teacherId), CancellationToken.None);

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
        var teacherId = Guid.NewGuid();

        _classRepositoryMock.Setup(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Class)null!);

        // Act
        var result = await _handler.Handle(new ChangeClassTeacherCommand(classId, teacherId), CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Class.NotFound(classId), result.Error);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenTeacherDoesNotExist()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        var classEntity = Helpers.CreateTestClass(
            classId, 
            Guid.NewGuid(),
            Guid.NewGuid(),
            ClassType.Laboratory,
            [],
            DateTime.UtcNow.AddDays(1));

        _classRepositoryMock.Setup(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(classEntity);
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(teacherId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);

        // Act
        var result = await _handler.Handle(new ChangeClassTeacherCommand(classId, teacherId), CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Teacher.NotFound(teacherId), result.Error);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenTeacherIsNotEligible()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        var classEntity = Helpers.CreateTestClass(
            classId, 
            Guid.NewGuid(),
            Guid.NewGuid(),
            ClassType.Laboratory,
            [],
            DateTime.UtcNow.AddDays(1));
        var teacher = Helpers.CreateTestUser(
            teacherId,
            "teacher@example.com",
            "password-hash",
            "John",
            "Doe",
            "Student");

        _classRepositoryMock.Setup(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(classEntity);
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(teacherId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(teacher);

        // Act
        var result = await _handler.Handle(new ChangeClassTeacherCommand(classId, teacherId), CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Teacher.NotFound(teacherId), result.Error);
    }
    
    #endregion
}
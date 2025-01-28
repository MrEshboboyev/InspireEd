using FluentAssertions;
using InspireEd.Application.Faculties.Groups.Commands.TransferStudentBetweenGroups;
using InspireEd.Application.UnitTests.Faculties.Commands.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Repositories;
using Moq;

namespace InspireEd.Application.UnitTests.Faculties.Commands.Groups;

public class TransferStudentBetweenGroupsCommandHandlerTests
{
    #region Fields & Mock Setup

    private readonly Mock<IFacultyRepository> _facultyRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly TransferStudentBetweenGroupsCommandHandler _handler;

    public TransferStudentBetweenGroupsCommandHandlerTests()
    {
        _facultyRepositoryMock = new Mock<IFacultyRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new TransferStudentBetweenGroupsCommandHandler(
            _facultyRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    #endregion

    #region Test Methods

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenFacultyNotFound()
    {
        // Arrange
        var command = CreateCommand();

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(command.FacultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Faculty)null!); // Faculty not found

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Faculty.NotFound(command.FacultyId));
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSourceGroupNotFound()
    {
        // Arrange
        var command = CreateCommand();
        var targetGroupId = command.TargetGroupId;
        var faculty = CreateFaculty(targetGroupId: targetGroupId, withTargetGroup: true);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(command.FacultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Faculty.GroupDoesNotExist(command.SourceGroupId));
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenTargetGroupNotFound()
    {
        // Arrange
        var command = CreateCommand();
        var sourceGroupId = command.SourceGroupId;
        var faculty = CreateFaculty(sourceGroupId: sourceGroupId, withSourceGroup: true);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(command.FacultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Faculty.GroupDoesNotExist(command.TargetGroupId));
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRemovingStudentFails()
    {
        // Arrange
        var command = CreateCommand();
        var faculty = CreateFaculty(withSourceGroup: true, withTargetGroup: true);

        var sourceGroup = faculty.GetGroupById(command.SourceGroupId);
        sourceGroup?.RemoveStudent(command.StudentId);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(command.FacultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Faculty.GroupDoesNotExist(command.SourceGroupId));
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAddingStudentFails()
    {
        // Arrange
        var command = CreateCommand();
        var faculty = CreateFaculty(withSourceGroup: true, withTargetGroup: true);

        var sourceGroup = faculty.GetGroupById(command.SourceGroupId);
        sourceGroup?.RemoveStudent(command.StudentId);

        var targetGroup = faculty.GetGroupById(command.TargetGroupId);
        targetGroup?.AddStudent(command.StudentId);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(command.FacultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Faculty.GroupDoesNotExist(command.SourceGroupId));
    }

    [Fact]
    public async Task Handle_ShouldSucceed_WhenStudentTransferredSuccessfully()
    {
        // Arrange
        var command = CreateCommand();
        var sourceGroupId = command.SourceGroupId;
        var targetGroupId = command.TargetGroupId;
        var faculty = CreateFaculty(
            sourceGroupId,
            targetGroupId, 
            withSourceGroup: true,
            withTargetGroup: true);
        
        var sourceGroup = faculty.GetGroupById(sourceGroupId);
        sourceGroup?.AddStudent(command.StudentId);
        
        // var targetGroup = faculty.GetGroupById(targetGroupId);
        // targetGroup?.AddStudent(command.StudentId);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(command.FacultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        _unitOfWorkMock
            .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _facultyRepositoryMock.Verify(repo => repo.Update(faculty), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region Helpers

    private static TransferStudentBetweenGroupsCommand CreateCommand() =>
        new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

    private static Faculty CreateFaculty(
        Guid sourceGroupId = default,
        Guid targetGroupId = default,
        bool withSourceGroup = false,
        bool withTargetGroup = false)
    {
        var faculty = Helpers.CreateTestFaculty(Guid.NewGuid(), "Engineering");

        if (withSourceGroup)
        {
            faculty.AddGroup(sourceGroupId, GroupName.Create("Group A").Value);
        }

        if (withTargetGroup)
        {
            faculty.AddGroup(targetGroupId, GroupName.Create("Group B").Value);
        }

        return faculty;
    }

    #endregion
}
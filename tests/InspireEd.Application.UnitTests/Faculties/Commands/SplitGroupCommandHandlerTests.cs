using FluentAssertions;
using InspireEd.Application.Faculties.Commands.SplitGroup;
using InspireEd.Application.UnitTests.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Repositories;
using Moq;

namespace InspireEd.Application.UnitTests.Faculties.Commands;

public class SplitGroupCommandHandlerTests
{
    #region Fields & Mock Setup
    
    private readonly Mock<IFacultyRepository> _facultyRepositoryMock;
    private readonly Mock<IGroupRepository> _groupRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly SplitGroupCommandHandler _handler;

    public SplitGroupCommandHandlerTests()
    {
        _facultyRepositoryMock = new Mock<IFacultyRepository>();
        _groupRepositoryMock = new Mock<IGroupRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new SplitGroupCommandHandler(
            _facultyRepositoryMock.Object,
            _groupRepositoryMock.Object,
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
            .ReturnsAsync((Faculty)null!);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Faculty.NotFound(command.FacultyId));
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenGroupNotFound()
    {
        // Arrange
        var command = CreateCommand();
        var faculty = CreateFaculty();

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdWithGroupsAsync(command.FacultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Faculty.GroupDoesNotExist(command.GroupId));
    }

    [Fact]
    public async Task Handle_ShouldSucceed_WhenGroupSplitSuccessfully()
    {
        // Arrange
        var command = CreateCommand();
        var faculty = CreateFaculty(
            command.FacultyId,
            command.GroupId,
            withGroup: true);
        
        // adding student this group
        var group = faculty.GetGroupById(command.GroupId);
        
        // 3 students added to this group
        group.AddStudent(Guid.NewGuid());
        group.AddStudent(Guid.NewGuid());
        group.AddStudent(Guid.NewGuid());

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
        _groupRepositoryMock.Verify(repo => repo.Add(It.IsAny<Group>()), Times.AtLeastOnce);
        _facultyRepositoryMock.Verify(repo => repo.Update(faculty), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    #endregion

    #region Helpers

    private static SplitGroupCommand CreateCommand() =>
        new(Guid.NewGuid(), Guid.NewGuid(), 2);

    private static Faculty CreateFaculty(
        Guid facultyId = default,
        Guid groupId = default,
        bool withGroup = false)
    {
        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering");

        if (!withGroup) return faculty;
        
        faculty.AddGroup(groupId, GroupName.Create("Group 1").Value);

        return faculty;
    }

    #endregion
}
using FluentAssertions;
using InspireEd.Application.Faculties.Commands.MergeGroups;
using InspireEd.Application.UnitTests.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Repositories;
using Moq;

namespace InspireEd.Application.UnitTests.Faculties.Commands;

public class MergeGroupsCommandHandlerTests
{
    #region Fields & Mock Setup
    
    private readonly Mock<IFacultyRepository> _facultyRepositoryMock;
    private readonly Mock<IGroupRepository> _groupRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly MergeGroupsCommandHandler _handler;

    public MergeGroupsCommandHandlerTests()
    {
        _facultyRepositoryMock = new Mock<IFacultyRepository>();
        _groupRepositoryMock = new Mock<IGroupRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new MergeGroupsCommandHandler(
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
    public async Task Handle_ShouldReturnFailure_WhenSomeGroupsAreNotFound()
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
        result.Error.Should().Be(DomainErrors.Faculty.SomeGroupsNotFound);
    }

    [Fact]
    public async Task Handle_ShouldSucceed_WhenGroupsMergedSuccessfully()
    {
        // Arrange
        var command = CreateCommand();
        var faculty = CreateFaculty(
            command.FacultyId,
            command.GroupIds[0],
            command.GroupIds[1],
            true);
        
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
        _groupRepositoryMock.Verify(repo => repo.Add(It.IsAny<Group>()), Times.Once);
        _facultyRepositoryMock.Verify(repo => repo.Update(faculty), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    #endregion

    #region Helpers

    private static MergeGroupsCommand CreateCommand() =>
        new(Guid.NewGuid(), [Guid.NewGuid(), Guid.NewGuid()]);

    private static Faculty CreateFaculty(
        Guid facultyId = default,
        Guid groupId1 = default,
        Guid groupId2 = default,
        bool withGroups = false)
    {
        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering");

        if (!withGroups) return faculty;
        
        faculty.AddGroup(groupId1, GroupName.Create("Group 1").Value);
        faculty.AddGroup(groupId2, GroupName.Create("Group 2").Value);

        return faculty;
    }

    #endregion
}
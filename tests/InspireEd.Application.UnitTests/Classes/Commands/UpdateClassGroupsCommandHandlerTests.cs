using InspireEd.Application.Classes.Commands.UpdateClassGroups;
using InspireEd.Application.UnitTests.Common;
using InspireEd.Domain.Classes.Entities;
using InspireEd.Domain.Classes.Enums;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Repositories;
using Moq;

namespace InspireEd.Application.UnitTests.Classes.Commands;

public class UpdateClassGroupsCommandHandlerTests
{
    #region Fields & Mock Setup

    private readonly Mock<IClassRepository> _classRepositoryMock;
    private readonly Mock<IGroupRepository> _groupRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateClassGroupsCommandHandler _handler;

    public UpdateClassGroupsCommandHandlerTests()
    {
        _classRepositoryMock = new Mock<IClassRepository>();
        _groupRepositoryMock = new Mock<IGroupRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new UpdateClassGroupsCommandHandler(
            _classRepositoryMock.Object,
            _groupRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    #endregion

    #region Test Methods

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenClassAndGroupsExist()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var groupIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        
        var classEntity = Helpers.CreateTestClass(
            classId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            ClassType.Laboratory,
            [],
            DateTime.UtcNow.AddDays(1)); 
        
        var faculty = Helpers.CreateTestFaculty(
            Guid.NewGuid(),
            "faculty-name");
        List<Group> groupEntities = [
            faculty.AddGroup(
                    groupIds[0], GroupName.Create("Group Name").Value)
                .Value,
            faculty.AddGroup(
                    groupIds[1], GroupName.Create("Group Name B").Value)
                .Value];

        _classRepositoryMock.Setup(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(classEntity);
        _groupRepositoryMock.Setup(repo => repo.GetByIdsAsync(groupIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync(groupEntities);

        // Act
        var result = await _handler.Handle(new UpdateClassGroupsCommand(classId, groupIds), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _classRepositoryMock.Verify(repo => repo.Update(It.IsAny<Class>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenClassDoesNotExist()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var groupIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        _classRepositoryMock.Setup(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Class)null!);

        // Act
        var result = await _handler.Handle(new UpdateClassGroupsCommand(classId, groupIds), CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Class.NotFound(classId), result.Error);
        _classRepositoryMock.Verify(repo => repo.Update(It.IsAny<Class>()), Times.Never);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSomeGroupsAreMissing()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var existingGroupId = Guid.NewGuid();
        var missingGroupId = Guid.NewGuid();
        var groupIds = new List<Guid> { existingGroupId, missingGroupId };
        
        var classEntity = Helpers.CreateTestClass(
            classId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            ClassType.Laboratory,
            [],
            DateTime.UtcNow.AddDays(1)); 
        
        var faculty = Helpers.CreateTestFaculty(
            Guid.NewGuid(),
            "faculty-name");
        
        var existingGroups = new List<Group>
        { 
            faculty.AddGroup(existingGroupId, GroupName.Create("Existing Group").Value).Value 
        };

        _classRepositoryMock.Setup(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(classEntity);
        _groupRepositoryMock.Setup(repo => repo.GetByIdsAsync(groupIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingGroups);

        // Act
        var result = await _handler.Handle(new UpdateClassGroupsCommand(classId, groupIds), CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Group.MissingGroups(new List<Guid> { missingGroupId }), result.Error);
        _classRepositoryMock.Verify(repo => repo.Update(It.IsAny<Class>()), Times.Never);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUpdatingGroupsFails()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var groupIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        
        var classEntity = Helpers.CreateTestClass(
            classId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            ClassType.Laboratory,
            [],
            DateTime.UtcNow.AddDays(1)); 
        
        var faculty = Helpers.CreateTestFaculty(
            Guid.NewGuid(),
            "faculty-name");

        List<Group> groupEntities = [
            faculty.AddGroup(
                    groupIds[0], GroupName.Create("Group Name").Value)
                .Value,
            faculty.AddGroup(
                    groupIds[0], GroupName.Create("Group Name B").Value)
                .Value];
        
        _classRepositoryMock.Setup(repo => repo.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(classEntity);
        _groupRepositoryMock.Setup(repo => repo.GetByIdsAsync(groupIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync(groupEntities);

        // **Manually invoke failure scenario**
        classEntity.UpdateGroups(groupIds); // This would normally succeed
        Helpers.CreateTestClass(
            classId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            ClassType.Laboratory,
            [],
            DateTime.UtcNow.AddDays(1)); 

        // Act
        var result = await _handler.Handle(new UpdateClassGroupsCommand(classId, groupIds), CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Group.MissingGroups([groupIds[1]]), result.Error);
        _classRepositoryMock.Verify(repo => repo.Update(It.IsAny<Class>()), Times.Never);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    #endregion
}

using InspireEd.Application.Faculties.Commands.RemoveDepartmentHeadFromFaculty;
using InspireEd.Application.UnitTests.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Users.Entities;
using InspireEd.Domain.Users.Repositories;
using Moq;

namespace InspireEd.Application.UnitTests.Faculties.Commands;

public class RemoveDepartmentHeadFromFacultyCommandHandlerTests
{
    #region Fields & Mock Setup

    private readonly Mock<IFacultyRepository> _facultyRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly RemoveDepartmentHeadFromFacultyCommandHandler _handler;

    public RemoveDepartmentHeadFromFacultyCommandHandlerTests()
    {
        _handler = new RemoveDepartmentHeadFromFacultyCommandHandler(
            _facultyRepositoryMock.Object,
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    #endregion

    #region Test Methods

    [Fact]
    public async Task Handle_Should_RemoveDepartmentHeadFromFaculty_Successfully()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var departmentHeadId = Guid.NewGuid();
        var command = new RemoveDepartmentHeadFromFacultyCommand(facultyId, departmentHeadId);

        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering Faculty");
        var departmentHead = Helpers.CreateTestUser(
            departmentHeadId, 
            "user@gmail.com",
            "password-hash",
            "firstname",
            "lastname",
            "DepartmentHead");
        
        faculty.AddDepartmentHead(departmentHead.Id);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdWithRolesAsync(departmentHeadId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(departmentHead);

        _facultyRepositoryMock
            .Setup(repo => repo.Update(faculty));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetByIdWithRolesAsync(departmentHeadId, It.IsAny<CancellationToken>()), Times.Once);
        _facultyRepositoryMock.Verify(repo => repo.Update(faculty), Times.Once);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_FacultyNotFound()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var departmentHeadId = Guid.NewGuid();
        var command = new RemoveDepartmentHeadFromFacultyCommand(facultyId, departmentHeadId);

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Faculty?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Faculty.NotFound(facultyId), result.Error);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetByIdWithRolesAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _facultyRepositoryMock.Verify(repo => repo.Update(It.IsAny<Faculty>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_DepartmentHeadNotFound()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var departmentHeadId = Guid.NewGuid();
        var command = new RemoveDepartmentHeadFromFacultyCommand(facultyId, departmentHeadId);

        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering Faculty");

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdWithRolesAsync(departmentHeadId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.DepartmentHead.NotFound(departmentHeadId), result.Error);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetByIdWithRolesAsync(departmentHeadId, It.IsAny<CancellationToken>()), Times.Once);
        _facultyRepositoryMock.Verify(repo => repo.Update(It.IsAny<Faculty>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_DepartmentHeadIsNotInRole()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var departmentHeadId = Guid.NewGuid();
        var command = new RemoveDepartmentHeadFromFacultyCommand(facultyId, departmentHeadId);

        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering Faculty");
        var departmentHead = Helpers.CreateTestUser(
            departmentHeadId, 
            "user@gmail.com",
            "password-hash",
            "firstname",
            "lastname",
            "Student");

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdWithRolesAsync(departmentHeadId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(departmentHead);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.DepartmentHead.NotFound(departmentHeadId), result.Error);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetByIdWithRolesAsync(departmentHeadId, It.IsAny<CancellationToken>()), Times.Once);
        _facultyRepositoryMock.Verify(repo => repo.Update(It.IsAny<Faculty>()), Times.Never);
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_RemoveDepartmentHeadFails()
    {
        // Arrange
        var facultyId = Guid.NewGuid();
        var departmentHeadId = Guid.NewGuid();
        var command = new RemoveDepartmentHeadFromFacultyCommand(facultyId, departmentHeadId);

        var faculty = Helpers.CreateTestFaculty(facultyId, "Engineering Faculty");
        var departmentHead = Helpers.CreateTestUser(
            departmentHeadId, 
            "user@gmail.com",
            "password-hash",
            "firstname",
            "lastname",
            "DepartmentHead");

        _facultyRepositoryMock
            .Setup(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(faculty);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdWithRolesAsync(departmentHeadId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(departmentHead);

        // Ensure that the department head ID is NOT in the faculty's department head IDs
        // This will cause RemoveDepartmentHead to fail
        faculty.RemoveDepartmentHead(departmentHeadId); // Remove the ID if it exists

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Faculty.DepartmentHeadIdDoesNotExist(departmentHeadId), result.Error);
        _facultyRepositoryMock.Verify(repo => repo.GetByIdAsync(facultyId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetByIdWithRolesAsync(departmentHeadId, It.IsAny<CancellationToken>()), Times.Once);
        _facultyRepositoryMock.Verify(repo => repo.Update(It.IsAny<Faculty>()), Times.Never); // Update should not be called
        _unitOfWorkMock.Verify(unit => unit.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never); // SaveChanges should not be called
    }

    #endregion
}

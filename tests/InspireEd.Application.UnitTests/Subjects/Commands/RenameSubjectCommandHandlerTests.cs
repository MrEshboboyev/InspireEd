using InspireEd.Application.Subjects.Commands.RenameSubject;
using InspireEd.Application.UnitTests.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Subjects.Entities;
using InspireEd.Domain.Subjects.Repositories;
using InspireEd.Domain.Subjects.ValueObjects;
using Moq;

namespace InspireEd.Application.UnitTests.Subjects.Commands;

public class RenameSubjectCommandHandlerTests
{
    #region Fields & Mock Setup
    
    private readonly Mock<ISubjectRepository> _subjectRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly RenameSubjectCommandHandler _handler;

    public RenameSubjectCommandHandlerTests()
    {
        _subjectRepositoryMock = new Mock<ISubjectRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new RenameSubjectCommandHandler(
            _subjectRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }
    
    #endregion
    
    #region Test Methods

    [Fact]
    public async Task Handle_SubjectNotFound_ReturnsFailure()
    {
        var command = new RenameSubjectCommand(Guid.NewGuid(), "New Name");
        _subjectRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Subject)null!);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Subject.NotFound(command.Id), result.Error);
    }

    [Fact]
    public async Task Handle_NameAlreadyInUse_ReturnsFailure()
    {
        var command = new RenameSubjectCommand(Guid.NewGuid(), "New Name");
        var subject = Helpers.CreateTestSubject(
            command.Id, 
            command.NewName,
            "code",
            4);

        _subjectRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subject);
        _subjectRepositoryMock.Setup(repo => repo.IsNameUniqueAsync(It.IsAny<SubjectName>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Subject.NameAlreadyInUse, result.Error);
    }

    [Fact]
    public async Task Handle_ValidRequest_RenamesSubjectAndSavesChanges()
    {
        var command = new RenameSubjectCommand(Guid.NewGuid(), "New Name");
        var subject = Helpers.CreateTestSubject(
            command.Id, 
            command.NewName,
            "code",
            4);

        _subjectRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subject);
        _subjectRepositoryMock.Setup(repo => repo.IsNameUniqueAsync(It.IsAny<SubjectName>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        _subjectRepositoryMock.Verify(repo => repo.Update(subject), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    #endregion
}
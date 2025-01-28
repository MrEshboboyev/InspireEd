using InspireEd.Application.Subjects.Commands.ChangeSubjectCode;
using InspireEd.Application.UnitTests.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Subjects.Entities;
using InspireEd.Domain.Subjects.Repositories;
using InspireEd.Domain.Subjects.ValueObjects;
using Moq;

namespace InspireEd.Application.UnitTests.Subjects.Commands;

public class ChangeSubjectCodeCommandHandlerTests
{
    #region Fields & Mock Setup
    
    private readonly Mock<ISubjectRepository> _subjectRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ChangeSubjectCodeCommandHandler _handler;

    public ChangeSubjectCodeCommandHandlerTests()
    {
        _subjectRepositoryMock = new Mock<ISubjectRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new ChangeSubjectCodeCommandHandler(
            _subjectRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }
    
    #endregion
    
    #region Test Methods

    [Fact]
    public async Task Handle_SubjectNotFound_ReturnsFailure()
    {
        var command = new ChangeSubjectCodeCommand(Guid.NewGuid(), "NEW123");

        _subjectRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Subject)null!);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Subject.NotFound(command.Id), result.Error);
    }

    [Fact]
    public async Task Handle_CodeAlreadyInUse_ReturnsFailure()
    {
        var command = new ChangeSubjectCodeCommand(Guid.NewGuid(), "NEW123");
        var subject = Helpers.CreateTestSubject(
            command.Id, 
            "subject-name",
            command.NewCode,
            4);

        _subjectRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subject);
        _subjectRepositoryMock.Setup(repo => repo.IsCodeUniqueAsync(It.IsAny<SubjectCode>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Subject.CodeAlreadyInUse, result.Error);
    }

    [Fact]
    public async Task Handle_ValidRequest_ChangesCodeAndSavesChanges()
    {
        var command = new ChangeSubjectCodeCommand(Guid.NewGuid(), "NEW123");
        var subject = Helpers.CreateTestSubject(
            command.Id, 
            "subject-name",
            command.NewCode,
            4);

        _subjectRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subject);
        _subjectRepositoryMock.Setup(repo => repo.IsCodeUniqueAsync(It.IsAny<SubjectCode>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        _subjectRepositoryMock.Verify(repo => repo.Update(subject), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    #endregion
}
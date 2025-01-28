using InspireEd.Application.Subjects.Commands.ChangeSubjectCredit;
using InspireEd.Application.UnitTests.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Subjects.Entities;
using InspireEd.Domain.Subjects.Repositories;
using Moq;

namespace InspireEd.Application.UnitTests.Subjects.Commands;

public class ChangeSubjectCreditCommandHandlerTests
{
    #region Fields & Mock Setup
    
    private readonly Mock<ISubjectRepository> _subjectRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ChangeSubjectCreditCommandHandler _handler;

    public ChangeSubjectCreditCommandHandlerTests()
    {
        _subjectRepositoryMock = new Mock<ISubjectRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new ChangeSubjectCreditCommandHandler(
            _subjectRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }
    
    #endregion
    
    #region Test Methods

    [Fact]
    public async Task Handle_SubjectNotFound_ReturnsFailure()
    {
        var command = new ChangeSubjectCreditCommand(Guid.NewGuid(), 4);

        _subjectRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Subject)null!);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Subject.NotFound(command.Id), result.Error);
    }

    [Fact]
    public async Task Handle_ValidRequest_ChangesCreditAndSavesChanges()
    {
        var command = new ChangeSubjectCreditCommand(Guid.NewGuid(), 4);
        var subject = Helpers.CreateTestSubject(
            command.Id, 
            "subject-name",
            "code",
            command.NewCredit);

        _subjectRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subject);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        _subjectRepositoryMock.Verify(repo => repo.Update(subject), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    #endregion
}
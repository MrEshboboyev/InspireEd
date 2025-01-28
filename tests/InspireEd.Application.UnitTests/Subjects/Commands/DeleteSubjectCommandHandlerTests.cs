using FluentAssertions;
using InspireEd.Application.Subjects.Commands.DeleteSubject;
using InspireEd.Application.UnitTests.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Subjects.Entities;
using InspireEd.Domain.Subjects.Repositories;
using Moq;

namespace InspireEd.Application.UnitTests.Subjects.Commands;

public class DeleteSubjectCommandHandlerTests
{
    #region Fields & Mock Setup
    
    private readonly Mock<ISubjectRepository> _subjectRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteSubjectCommandHandler _handler;

    public DeleteSubjectCommandHandlerTests()
    {
        _subjectRepositoryMock = new Mock<ISubjectRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new DeleteSubjectCommandHandler(
            _subjectRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }
    
    #endregion

    #region Test Methods
    
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSubjectNotFound()
    {
        var command = new DeleteSubjectCommand(Guid.NewGuid());

        _subjectRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.SubjectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Subject)null!);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Subject.NotFound(command.SubjectId));
    }

    [Fact]
    public async Task Handle_ShouldSucceed_WhenSubjectIsDeletedSuccessfully()
    {
        var subject = Helpers.CreateTestSubject(
            Guid.NewGuid(),
            "History", 
            "HIST101",
            3);
        var command = new DeleteSubjectCommand(subject.Id);

        _subjectRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.SubjectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subject);

        _unitOfWorkMock
            .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _subjectRepositoryMock.Verify(repo => repo.Remove(subject), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    #endregion
}
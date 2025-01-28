using FluentAssertions;
using InspireEd.Application.Subjects.Commands.UpdateSubject;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Subjects.Entities;
using InspireEd.Domain.Subjects.Repositories;
using InspireEd.Domain.Subjects.ValueObjects;
using Moq;

namespace InspireEd.Application.UnitTests.Subjects.Commands;

public class UpdateSubjectCommandHandlerTests
{
    #region Fields & Mock Setup
    
    private readonly Mock<ISubjectRepository> _subjectRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateSubjectCommandHandler _handler;

    public UpdateSubjectCommandHandlerTests()
    {
        _subjectRepositoryMock = new Mock<ISubjectRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new UpdateSubjectCommandHandler(
            _subjectRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }
    
    #endregion
    
    #region Test Methods

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSubjectNotFound()
    {
        var command = CreateCommand();

        _subjectRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Subject)null!);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Subject.NotFound(command.Id));
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNameIsNotUnique()
    {
        var command = CreateCommand();
        var subject = CreateSubject(
            command.Id,
            command.Name,
            command.Code,
            command.Credit);

        _subjectRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subject);

        _subjectRepositoryMock
            .Setup(repo => repo.IsNameUniqueAsync(subject.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Subject.NameAlreadyInUse);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCodeIsNotUnique()
    {
        var command = CreateCommand();
        var subject = CreateSubject(
            command.Id,
            command.Name,
            command.Code,
            command.Credit);

        _subjectRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subject);
        
        
        _subjectRepositoryMock
            .Setup(repo => repo.IsNameUniqueAsync(subject.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _subjectRepositoryMock
            .Setup(repo => repo.IsCodeUniqueAsync(subject.Code, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Subject.CodeAlreadyInUse);
    }

    [Fact]
    public async Task Handle_ShouldSucceed_WhenSubjectIsUpdatedSuccessfully()
    {
        var command = CreateCommand();
        var subject = CreateSubject(
            command.Id,
            command.Name,
            command.Code,
            command.Credit);

        _subjectRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subject);

        _subjectRepositoryMock
            .Setup(repo => repo.IsNameUniqueAsync(subject.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _subjectRepositoryMock
            .Setup(repo => repo.IsCodeUniqueAsync(subject.Code, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _unitOfWorkMock
            .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _subjectRepositoryMock.Verify(repo => repo.Update(It.IsAny<Subject>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    #endregion

    #region Helpers
    
    private static UpdateSubjectCommand CreateCommand() =>
        new(Guid.NewGuid(), "Physics", "PHYS101", 4);

    private static Subject CreateSubject(
        Guid id,
        string subjectName,
        string subjectCode,
        int subjectCredit) =>
        Subject.Create(
            id,
            SubjectName.Create(subjectName).Value,
            SubjectCode.Create(subjectCode).Value,
            SubjectCredit.Create(subjectCredit).Value);
    
    #endregion
}
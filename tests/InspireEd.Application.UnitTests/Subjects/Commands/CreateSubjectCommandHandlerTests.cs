using FluentAssertions;
using InspireEd.Application.Subjects.Commands.CreateSubject;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Subjects.Entities;
using InspireEd.Domain.Subjects.Repositories;
using InspireEd.Domain.Subjects.ValueObjects;
using Moq;

namespace InspireEd.Application.UnitTests.Subjects.Commands;

public class CreateSubjectCommandHandlerTests
{
    #region Fields & Mock Setup
    
    private readonly Mock<ISubjectRepository> _subjectRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateSubjectCommandHandler _handler;

    public CreateSubjectCommandHandlerTests()
    {
        _subjectRepositoryMock = new Mock<ISubjectRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new CreateSubjectCommandHandler(
            _subjectRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }
    
    #endregion
    
    #region Test Methods

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNameIsNotUnique()
    {
        var command = CreateCommand();
        var subjectName = SubjectName.Create(command.Name).Value;

        _subjectRepositoryMock
            .Setup(repo => repo.IsNameUniqueAsync(subjectName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Subject.NameAlreadyInUse);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCodeIsNotUnique()
    {
        var command = CreateCommand();
        var subjectName = SubjectName.Create(command.Name).Value;
        var subjectCode = SubjectCode.Create(command.Code).Value;

        _subjectRepositoryMock
            .Setup(repo => repo.IsNameUniqueAsync(subjectName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _subjectRepositoryMock
            .Setup(repo => repo.IsCodeUniqueAsync(subjectCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Subject.CodeAlreadyInUse);
    }

    [Fact]
    public async Task Handle_ShouldSucceed_WhenSubjectIsCreatedSuccessfully()
    {
        var command = CreateCommand();
        var subjectName = SubjectName.Create(command.Name).Value;
        var subjectCode = SubjectCode.Create(command.Code).Value;

        _subjectRepositoryMock
            .Setup(repo => repo.IsNameUniqueAsync(subjectName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _subjectRepositoryMock
            .Setup(repo => repo.IsCodeUniqueAsync(subjectCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _unitOfWorkMock
            .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _subjectRepositoryMock.Verify(repo => repo.Add(It.IsAny<Subject>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    #endregion

    #region Helpers 
    
    private static CreateSubjectCommand CreateCommand() =>
        new("Mathematics", "MATH101", 3);
    
    #endregion
}
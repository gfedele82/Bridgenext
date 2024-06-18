using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Engine;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Schema.DB;
using Bridgenext.Test.Builders;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;
using Bridgenext.DataAccess.DTOAdapter;

namespace Bridgenext.Test.UnitTest.Engines
{
    [TestFixture]
    public class CommentEngineTest
    {
        private Mock<ILogger<CommentEngine>> _logger;
        private Mock<IValidator<CreateCommetRequest>> _createCommentRequestValidator;
        private Mock<IValidator<DeleteCommetRequest>> _deleteCommentRequestValidator;
        private Mock<ICommentRepository> _commentRepository;
        private Mock<IDocumentRepositoty> _documentRepository;
        private Mock<IUserRepository> _userRepository;
        private CommentEngine _sut;
        private CreateCommetRequest _createCommetRequest;
        private DeleteCommetRequest _deleteCommetRequest;
        private CommentTestBuilder _commentTestBuilder;
        private DocumentTestBuilder _documentTestBuilder;
        private UserTestBuilder _userTestBuilder;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<CommentEngine>>();
            _createCommentRequestValidator = new Mock<IValidator<CreateCommetRequest>>();
            _deleteCommentRequestValidator = new Mock<IValidator<DeleteCommetRequest>>();
            _commentRepository = new Mock<ICommentRepository>();
            _documentRepository = new Mock<IDocumentRepositoty>();
            _userRepository = new Mock<IUserRepository>();
            _sut = new CommentEngine(_logger.Object, 
                _createCommentRequestValidator.Object, 
                _deleteCommentRequestValidator.Object, 
                _commentRepository.Object, 
                _documentRepository.Object, 
                _userRepository.Object);
            _commentTestBuilder = new CommentTestBuilder();
            _documentTestBuilder = new DocumentTestBuilder();
            _userTestBuilder = new UserTestBuilder();
            _createCommetRequest = _commentTestBuilder.CreateBuild();
            _deleteCommetRequest = _commentTestBuilder.DeleteBuild();
        }

        [Test]
        public void Given_ACreateCommentRequest_When_ValidationFails_Then_AnExceptionShallBeCaptures()
        {
            var exception = new ValidationException("Test");
            _createCommentRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.CreateComment(_createCommetRequest));

            ClassicAssert.That(exceptionReceived.Message.Equals(exception.Message));
            _commentRepository.Verify(x => x.InsertAsync(It.IsAny<Comments>()), Times.Never);
            _userRepository.Verify(x => x.GetAllByEmail(It.IsAny<string>()), Times.Never);
            _documentRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Test]
        public void Given_ADeleteCommentRequest_When_ValidationFails_Then_AnExceptionShallBeCaptures()
        {
            var exception = new ValidationException("Test");
            _deleteCommentRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.DeleteComment(_deleteCommetRequest));

            ClassicAssert.That(exceptionReceived.Message.Equals(exception.Message));
            _commentRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Never);
            _commentRepository.Verify(x => x.DeleteAsync(It.IsAny<Comments>()), Times.Never);
        }

        [Test]
        public async Task Given_NewCreateCommentRequest_When_AllValidationsPass_Then_DataAccessIsCalledToInsert_And_Validates_OneTransactionAdded()
        {
            var user = _userTestBuilder.DbBuild();
            var document = _documentTestBuilder.DbBuild();
            var expectedDB = _createCommetRequest.ToDatabaseModel(document,user);
            List<Users> listUser = [user];

            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(document);
            _userRepository.Setup(x => x.GetAllByEmail(It.IsAny<string>())).ReturnsAsync(listUser);
            _commentRepository.Setup(x => x.InsertAsync(It.IsAny<Comments>())).ReturnsAsync(expectedDB).Verifiable();
            _createCommentRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();

            await _sut.CreateComment(_createCommetRequest);

            _commentRepository.Verify(x => x.InsertAsync(It.IsAny<Comments>()), Times.Once);
        }

        [Test]
        public async Task Given_NewDeleteCommentRequest_When_AllValidationsPass_Then_DataAccessIsCalledToDelete_And_Validates_OneTransactionDeleted()
        {
            var user = _userTestBuilder.DbBuild();
            var document = _documentTestBuilder.DbBuild();
            var comment = _commentTestBuilder.CreateBuild();
            comment.IdDocument = _deleteCommetRequest.Id;
            var expectedDB = comment.ToDatabaseModel(document, user);

            _commentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(expectedDB);
            _commentRepository.Setup(x => x.DeleteAsync(It.IsAny<Comments>())).Verifiable();
            _deleteCommentRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();

            await _sut.DeleteComment(_deleteCommetRequest);

            _commentRepository.Verify(x => x.DeleteAsync(It.IsAny<Comments>()), Times.Once);
        }

        [Test]
        public async Task Given_GetById_When_AllValidationsPass_Then_DataAccessIsCalledToInsert_And_Validates_ReturnedData()
        {
            var user = _userTestBuilder.DbBuild();
            var document = _documentTestBuilder.DbBuild();
            var comment = _commentTestBuilder.CreateBuild();
            comment.IdDocument = _deleteCommetRequest.Id;
            var expectedDB = comment.ToDatabaseModel(document, user);

            _commentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(expectedDB);

            var response = await _sut.GetCommentById(comment.IdDocument);

            ClassicAssert.That(response.Id == expectedDB.Id);
        }

        [Test]
        public async Task Given_GetAll_When_AllValidationsPass_Then_DataAccessIsCalledToInsert_And_Validates_ReturnedData()
        {
            var user = _userTestBuilder.DbBuild();
            var document = _documentTestBuilder.DbBuild();
            var comment = _commentTestBuilder.CreateBuild();
            comment.IdDocument = _deleteCommetRequest.Id;
            var expectedDB = comment.ToDatabaseModel(document, user);
            List<Comments> listComments = [expectedDB];

            _commentRepository.Setup(x => x.GetAll()).ReturnsAsync(listComments);

            var response = await _sut.GetAllComments();

            ClassicAssert.That(response.Count > 0);
        }

    }
}

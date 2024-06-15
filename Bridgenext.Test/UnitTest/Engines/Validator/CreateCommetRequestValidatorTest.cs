using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Engine.Validators;
using Bridgenext.Models.Constant.Exceptions;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Test.Builders;
using FluentValidation;
using Moq;
using NUnit.Framework.Legacy;

namespace Bridgenext.Test.UnitTest.Engines.Validator
{
    [TestFixture]
    public class CreateCommetRequestValidatorTest
    {
        private IValidator<CreateCommetRequest> _sut;
        private CreateCommetRequest _request;
        private CommentTestBuilder _builder;
        private Mock<IDocumentRepositoty> _documentRepositoty;
        private Mock<IUserRepository> _userRepository;

        [SetUp]
        public void Setup()
        {
            _documentRepositoty = new Mock<IDocumentRepositoty>();
            _userRepository = new Mock<IUserRepository>();
            _sut = new CreateCommetRequestValidator(_userRepository.Object, _documentRepositoty.Object);
            _builder = new CommentTestBuilder();
            _request = _builder.CreateBuild();

        }

        [Test]
        public async Task Given_ValidPayload_With_ValidCreateComment_WhenInvokeValidator_Then_ItShouldPassValidation()
        {
            _documentRepositoty.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            _userRepository.Setup(x => x.IdExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            await _sut.ValidateAndThrowAsync(_request);
            ClassicAssert.Pass();
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyIdDocument_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            var exceptionMessage = CommentExceptions.RequiredIdDocument;

            _request.IdDocument = Guid.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyDocumentNotExist_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            var exceptionMessage = CommentExceptions.DocumentNotExist;

            _documentRepositoty.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyContent_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            var exceptionMessage = CommentExceptions.RequiredContent;

            _documentRepositoty.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _request.Content = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyCreateUser_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            var exceptionMessage = CommentExceptions.CreateUserNotExist;

            _request.CreateUser = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_CreateUserNotExist_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            var exceptionMessage = CommentExceptions.CreateUserNotExist;

            _documentRepositoty.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            _userRepository.Setup(x => x.IdExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

            CaptureExceptionAndValidate(exceptionMessage);
        }


        private void CaptureExceptionAndValidate(string exceptionMessage)
        {
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.ValidateAndThrowAsync(_request));
            ClassicAssert.That(exceptionReceived.Message.Contains(exceptionMessage));
        }
    }
}

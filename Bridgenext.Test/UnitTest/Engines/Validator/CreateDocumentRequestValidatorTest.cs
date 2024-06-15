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
    public class CreateDocumentRequestValidatorTest
    {
        private IValidator<CreateDocumentRequest> _sut;
        private CreateDocumentRequest _request;
        private DocumentTestBuilder _builder;
        private Mock<IUserRepository> _userRepository;

        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();
            _sut = new CreateDocumentRequestValidator(_userRepository.Object);
            _builder = new DocumentTestBuilder();
            _request = _builder.BuildCreate();
        }

        [Test]
        public async Task Given_ValidPayload_With_ValidCreateDocument_WhenInvokeValidator_Then_ItShouldPassValidation()
        {
            _userRepository.Setup(x => x.IdExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            _request.File = null;

            await _sut.ValidateAndThrowAsync(_request);
            ClassicAssert.Pass();
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyName_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            var exceptionMessage = DocumentExceptions.RequiredName;

            _request.Name = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyDescription_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            var exceptionMessage = DocumentExceptions.RequiredDescription;

            _request.Description = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyFileAndContent_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            var exceptionMessage = DocumentExceptions.RequiredFileContentSameTime;

            _request.File = string.Empty;
            _request.Content = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_NotFileAndContent_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            var exceptionMessage = DocumentExceptions.RequiredFileContentSameTime;

            _request.File = "test";
            _request.Content = "test";

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyCreateUser_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            var exceptionMessage = DocumentExceptions.CreateUserNotExist;

            _request.File = null;
            _request.CreateUser = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_NotExistCreateUser_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            var exceptionMessage = DocumentExceptions.UserNotExist;
            _userRepository.Setup(x => x.IdExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

            _request.File = null;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_NotExistFile_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            var exceptionMessage = DocumentExceptions.FileNotExist;
            _userRepository.Setup(x => x.IdExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            _request.Content = null;

            _request.File = "test_test.txt";

            CaptureExceptionAndValidate(exceptionMessage);
        }

        private void CaptureExceptionAndValidate(string exceptionMessage)
        {
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.ValidateAndThrowAsync(_request));
            ClassicAssert.That(exceptionReceived.Message.Contains(exceptionMessage));
        }

    }
}

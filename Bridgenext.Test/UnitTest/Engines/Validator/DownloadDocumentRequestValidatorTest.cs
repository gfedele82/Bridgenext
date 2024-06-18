using Bridgenext.DataAccess.Interfaces;
using Bridgenext.DataAccess.Repositories;
using Bridgenext.Engine.Validators;
using Bridgenext.Models.Constant.Exceptions;
using Bridgenext.Models.Schema.DB;
using Bridgenext.Test.Builders;
using FluentValidation;
using Moq;
using NUnit.Framework.Legacy;

namespace Bridgenext.Test.UnitTest.Engines.Validator
{
    [TestFixture]
    public class DownloadDocumentRequestValidatorTest
    {
        private IValidator<Guid> _sut;
        private Guid _request;
        private DocumentTestBuilder _builder;
        private Mock<IDocumentRepositoty> _documentRepository;
        private Documents _document;

        [SetUp]
        public void Setup()
        {
            _documentRepository = new Mock<IDocumentRepositoty>();
            _sut = new DownloadDocumentRequestValidator(_documentRepository.Object);
            _builder = new DocumentTestBuilder();
            _request = Guid.NewGuid();
            _document = _builder.DbBuild();
            _document.TargetFile = "test.txt";
        }

        [Test]
        public async Task Given_ValidPayload_With_ValidDownloadDocument_WhenInvokeValidator_Then_ItShouldPassValidation()
        {
            _documentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_document);

            await _sut.ValidateAndThrowAsync(_request);
            ClassicAssert.Pass();
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyId_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _documentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_document);

            var exceptionMessage = DocumentExceptions.RequiredId;

            _request = Guid.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_NotExistId_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _documentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_document);

            var exceptionMessage = DocumentExceptions.DocumentNotExist;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_NotFileExist_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _documentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            _document.TargetFile = null;
            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_document);

            var exceptionMessage = DocumentExceptions.FileDoesNotHaveFile;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        private void CaptureExceptionAndValidate(string exceptionMessage)
        {
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.ValidateAndThrowAsync(_request));
            ClassicAssert.That(exceptionReceived.Message.Contains(exceptionMessage));
        }
    }
}

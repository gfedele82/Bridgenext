using Bridgenext.Engine.Validators;
using Bridgenext.Models.Constant.Exceptions;
using FluentValidation;
using NUnit.Framework.Legacy;

namespace Bridgenext.Test.UnitTest.Engines.Validator
{
    [TestFixture]
    public class EmailRequestValidatorTest
    {
        private IValidator<string> _sut;
        private string _request;

        [SetUp]
        public void Setup()
        {
            _sut = new EmailRequestValidator();
            _request = "test@test.com";
        }

        [Test]
        public async Task Given_ValidPayload_With_ValidEmail_WhenInvokeValidator_Then_ItShouldPassValidation()
        {
            await _sut.ValidateAndThrowAsync(_request);
            ClassicAssert.Pass();
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyEmail_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            var exceptionMessage = UserExceptions.RequiredEmail;

            _request = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_InvalidEmail_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            var exceptionMessage = UserExceptions.InvalidEmail;

            _request = "test";

            CaptureExceptionAndValidate(exceptionMessage);
        }

        private void CaptureExceptionAndValidate(string exceptionMessage)
        {
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.ValidateAndThrowAsync(_request));
            ClassicAssert.That(exceptionReceived.Message.Contains(exceptionMessage));
        }
    }
}

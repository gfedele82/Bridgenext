using Bridgenext.Engine.Validators;
using Bridgenext.Models.Constant.Exceptions;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Test.Builders;
using FluentValidation;
using NUnit.Framework.Legacy;

namespace Bridgenext.Test.UnitTest.Engines.Validator
{
    [TestFixture]
    public class CreateAddressRequestValidatorTest
    {
        private IValidator<CreateAddressRequest> _sut;
        private CreateAddressRequest _request;
        private AddressTestBuilder _builder;

        [SetUp]
        public void Setup()
        {
            _sut = new CreateAddressRequestValidator();
            _builder = new AddressTestBuilder();
            _request = _builder.CreateBuilder();

        }

        [Test]
        public async Task Given_ValidPayload_With_ValidCreateAddress_WhenInvokeValidator_Then_ItShouldPassValidation()
        {
            await _sut.ValidateAndThrowAsync(_request);
            ClassicAssert.Pass();
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyLine1_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            var exceptionMessage = AddressExceptions.RequiredLine1;

            _request.Line1 = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyCity_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            var exceptionMessage = AddressExceptions.RequiredCity;

            _request.City = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyCountry_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            var exceptionMessage = AddressExceptions.RequiredCountry;

            _request.Country = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyZip_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            var exceptionMessage = AddressExceptions.RequiredZip;

            _request.Zip = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyCreateUser_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            var exceptionMessage = AddressExceptions.CreateUserNotExist;

            _request.CreateUser = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        private void CaptureExceptionAndValidate(string exceptionMessage)
        {
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.ValidateAndThrowAsync(_request));
            ClassicAssert.That(exceptionReceived.Message.Contains(exceptionMessage));
        }
    }
}

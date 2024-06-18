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
    public class UpdateAddressRequestValidatorTest
    {
        private IValidator<UpdateAddressRequest> _sut;
        private UpdateAddressRequest _request;
        private AddressTestBuilder _builder;
        private Mock<IAddressRepository> _addressRepository;

        [SetUp]
        public void Setup()
        {
            _addressRepository = new Mock<IAddressRepository>();
            _sut = new UpdateAddressRequestValidator(_addressRepository.Object);
            _builder = new AddressTestBuilder();
            _request = _builder.UpdateBuilder();

        }

        [Test]
        public async Task Given_ValidPayload_With_ValidCreateAddress_WhenInvokeValidator_Then_ItShouldPassValidation()
        {
            _addressRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            await _sut.ValidateAndThrowAsync(_request);
            ClassicAssert.Pass();
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyId_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _addressRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            var exceptionMessage = AddressExceptions.RequiredIdAddress;

            _request.Id = Guid.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyLine1_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _addressRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            var exceptionMessage = AddressExceptions.RequiredLine1;

            _request.Line1 = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyCity_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _addressRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            var exceptionMessage = AddressExceptions.RequiredCity;

            _request.City = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyCountry_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _addressRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            var exceptionMessage = AddressExceptions.RequiredCountry;

            _request.Country = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyZip_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _addressRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            var exceptionMessage = AddressExceptions.RequiredZip;

            _request.Zip = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyModifyUser_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _addressRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            var exceptionMessage = AddressExceptions.CreateUserNotExist;

            _request.ModifyUser = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_NotExistAddress_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _addressRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            var exceptionMessage = AddressExceptions.AddressNotExist;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        private void CaptureExceptionAndValidate(string exceptionMessage)
        {
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.ValidateAndThrowAsync(_request));
            ClassicAssert.That(exceptionReceived.Message.Contains(exceptionMessage));
        }
    }
}

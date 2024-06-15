using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Engine.Validators;
using Bridgenext.Models.Constant.Exceptions;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Schema.DB;
using Bridgenext.Test.Builders;
using FluentValidation;
using Moq;
using NUnit.Framework.Legacy;
using System.Linq.Expressions;

namespace Bridgenext.Test.UnitTest.Engines.Validator
{
    [TestFixture]
    public class CreateUserRequestValidatorTest
    {
        private IValidator<CreateUserRequest> _sut;
        private CreateUserRequest _request;
        private Users _userAdmin;
        private List<Users> _listUser;
        private UserTestBuilder _builder;
        private Mock<IUserRepository> _userRepository;

        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();
            _sut = new CreateUserRequestValidator(_userRepository.Object);
            _builder = new UserTestBuilder();
            _request = _builder.CreateBuilder();
            _userAdmin = _builder.DbBuild();
            _listUser = [_userAdmin];
        }

        [Test]
        public async Task Given_ValidPayload_With_ValidCreateDocument_WhenInvokeValidator_Then_ItShouldPassValidation()
        {
            _userRepository.Setup(x => x.IdExistsAsync(_request.Email)).ReturnsAsync(false);

            _userRepository.Setup(x => x.IdExistsAsync(_request.CreateUser)).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            await _sut.ValidateAndThrowAsync(_request);
            ClassicAssert.Pass();
        }

         [Test]
         public void Given_InvalidPayload_With_EmptyFirstName_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
         {
            _userRepository.Setup(x => x.IdExistsAsync(_request.Email)).ReturnsAsync(false);

            _userRepository.Setup(x => x.IdExistsAsync(_request.CreateUser)).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = UserExceptions.RequiredFirstName;

             _request.FirstName = string.Empty;

             CaptureExceptionAndValidate(exceptionMessage);
         }

        [Test]
        public void Given_InvalidPayload_With_EmptyLastName_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _userRepository.Setup(x => x.IdExistsAsync(_request.Email)).ReturnsAsync(false);

            _userRepository.Setup(x => x.IdExistsAsync(_request.CreateUser)).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = UserExceptions.RequiredLastName;

            _request.LastName = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyEmail_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _userRepository.Setup(x => x.IdExistsAsync(_request.Email)).ReturnsAsync(false);

            _userRepository.Setup(x => x.IdExistsAsync(_request.CreateUser)).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = UserExceptions.RequiredEmail;

            _request.Email = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_InvalidIdUserType_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _userRepository.Setup(x => x.IdExistsAsync(_request.Email)).ReturnsAsync(false);

            _userRepository.Setup(x => x.IdExistsAsync(_request.CreateUser)).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = UserExceptions.RequiredUserType;

            _request.IdUserType = 3;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyCreateUser_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _userRepository.Setup(x => x.IdExistsAsync(_request.Email)).ReturnsAsync(false);

            _userRepository.Setup(x => x.IdExistsAsync(_request.CreateUser)).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = UserExceptions.CreateUserNotExist;

            _request.CreateUser = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_NotExistCreateUser_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _userRepository.Setup(x => x.IdExistsAsync(_request.Email)).ReturnsAsync(false);

            _userRepository.Setup(x => x.IdExistsAsync(_request.CreateUser)).ReturnsAsync(false);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = UserExceptions.CreateUserNotExist;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_InvalidEmail_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _userRepository.Setup(x => x.IdExistsAsync(_request.Email)).ReturnsAsync(false);

            _userRepository.Setup(x => x.IdExistsAsync(_request.CreateUser)).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = UserExceptions.InvalidEmail;

            _request.Email = "test";

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_ExistEmail_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _userRepository.Setup(x => x.IdExistsAsync(_request.Email)).ReturnsAsync(true);

            _userRepository.Setup(x => x.IdExistsAsync(_request.CreateUser)).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = UserExceptions.UserExist;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_NotAdminCreateUser_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _listUser.Clear();

            var _user = _builder.DbBuild();

            _user.IdUserType = 2;
            _user.UserTypes.Id = 2;

            _listUser = [_user];

            _userRepository.Setup(x => x.IdExistsAsync(_request.Email)).ReturnsAsync(false);

            _userRepository.Setup(x => x.IdExistsAsync(_request.CreateUser)).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = UserExceptions.CreateUserNotExist;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        private void CaptureExceptionAndValidate(string exceptionMessage)
        {
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.ValidateAndThrowAsync(_request));
            ClassicAssert.That(exceptionReceived.Message.Contains(exceptionMessage));
        }
    }
}

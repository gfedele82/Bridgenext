using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Engine.Validators;
using Bridgenext.Models.Constant.Exceptions;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Schema.DB;
using Bridgenext.Test.Builders;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework.Legacy;
using System.Linq.Expressions;

namespace Bridgenext.Test.UnitTest.Engines.Validator
{
    [TestFixture]
    public class DeleteUserRequestValidatorTest
    {
        private IValidator<DeleteUserRequest> _sut;
        private DeleteUserRequest _request;
        private UserTestBuilder _builder;
        private Mock<IUserRepository> _userRepository;
        private Mock<IConfigurationRoot> _configurationRoot;
        private Users _userAdmin;
        private List<Users> _listUser;
        private Users _userSystem;
        private readonly Guid _idAdmin = Guid.Parse("679bd613-da71-48b9-bf5c-b7b598935b77");

        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();
            _configurationRoot = new Mock<IConfigurationRoot>();
            _configurationRoot.Setup(x => x["IdUserAdmin"]).Returns(_idAdmin.ToString());
            _sut = new DeleteUserRequestValidator(_userRepository.Object, _configurationRoot.Object);
            _builder = new UserTestBuilder();
            _request = _builder.DeleteBuilder();
            _userAdmin = _builder.DbBuild();
            _userAdmin.Id = Guid.NewGuid();
            _listUser = [_userAdmin];
            _userSystem = _builder.DbBuild();
            _userSystem.Id = _idAdmin;
        }

        [Test]
        public async Task Given_ValidPayload_With_ValidUserDocument_WhenInvokeValidator_Then_ItShouldPassValidation()
        {
            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            await _sut.ValidateAndThrowAsync(_request);
            ClassicAssert.Pass();
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyId_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = UserExceptions.RequiredIdUser;

            _request.Id = Guid.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_UserAdmin_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = UserExceptions.CanNotDeleteUserAdmin;

            _request.Id = _idAdmin;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyUserModify_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = UserExceptions.CreateUserNotExist;

            _request.ModifyUser = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_NoAdminUserModify_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _listUser.Clear();
            var user = _builder.DbBuild();

            user.IdUserType = 2;
            user.UserTypes.Id = 2;

            _listUser = [user];

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

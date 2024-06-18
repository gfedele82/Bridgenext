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
    public class DisableDocumentRequestValidatorTest
    {
        private IValidator<DisableDocumentRequest> _sut;
        private DisableDocumentRequest _request;
        private DocumentTestBuilder _builder;
        private UserTestBuilder _userBuilder;
        private Mock<IUserRepository> _userRepository;
        private Mock<IDocumentRepositoty> _documentRepository;
        private Mock<IConfigurationRoot> _configurationRoot;
        private Users _userAdmin;
        private List<Users> _listUser;
        private readonly Guid _idAdmin = Guid.Parse("679bd613-da71-48b9-bf5c-b7b598935b77");

        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();
            _documentRepository = new Mock<IDocumentRepositoty>();
            _configurationRoot = new Mock<IConfigurationRoot>();
            _configurationRoot.Setup(x => x["IdUserAdmin"]).Returns(_idAdmin.ToString());
            _sut = new DisableDocumentRequestValidator(_userRepository.Object, _documentRepository.Object);
            _builder = new DocumentTestBuilder();
            _userBuilder = new UserTestBuilder();
            _request = _builder.BuildDisable();
            _userAdmin = _userBuilder.DbBuild();
            _userAdmin.Id = _idAdmin;
            _listUser = [_userAdmin];
        }

        [Test]
        public async Task Given_ValidPayload_With_ValidDisableDocument_WhenInvokeValidator_Then_ItShouldPassValidation()
        {
            _documentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);


            await _sut.ValidateAndThrowAsync(_request);
            ClassicAssert.Pass();
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyId_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _documentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = DocumentExceptions.RequiredId;

            _request.Id = Guid.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_NotExistId_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _documentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = DocumentExceptions.DocumentNotExist;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyModifyUser_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _documentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = DocumentExceptions.DocumentDisabled;

            _request.ModifyUser = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyComment_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _documentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = DocumentExceptions.RequiredComment;

            _request.Comment = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_NoAdminUserModify_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _documentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _listUser.Clear();
            var user = _userBuilder.DbBuild();

            user.IdUserType = 2;
            user.UserTypes.Id = 2;

            _listUser = [user];

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = DocumentExceptions.DocumentDisabled;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        private void CaptureExceptionAndValidate(string exceptionMessage)
        {
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.ValidateAndThrowAsync(_request));
            ClassicAssert.That(exceptionReceived.Message.Contains(exceptionMessage));
        }
    }
}

using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Engine.Validators;
using Bridgenext.Models.Constant.Exceptions;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Enums;
using Bridgenext.Models.Schema.DB;
using Bridgenext.Test.Builders;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework.Legacy;
using System.Linq.Expressions;


namespace Bridgenext.Test.UnitTest.Engines.Validator
{
    public class UpdateDocumentRequestValidatorTest
    {
        private IValidator<UpdateDocumentRequest> _sut;
        private UpdateDocumentRequest _request;
        private DocumentTestBuilder _builder;
        private UserTestBuilder _userBuilder;
        private Mock<IUserRepository> _userRepository;
        private Mock<IDocumentRepositoty> _documentRepository;
        private Mock<IConfigurationRoot> _configurationRoot;
        private Users _userAdmin;
        private List<Users> _listUser;
        private Documents _document;
        private readonly Guid _idAdmin = Guid.Parse("679bd613-da71-48b9-bf5c-b7b598935b77");

        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();
            _documentRepository = new Mock<IDocumentRepositoty>();
            _configurationRoot = new Mock<IConfigurationRoot>();
            _configurationRoot.Setup(x => x["IdUserAdmin"]).Returns(_idAdmin.ToString());
            _sut = new UpdateDocumentRequestValidator(_userRepository.Object, _documentRepository.Object, _configurationRoot.Object);
            _builder = new DocumentTestBuilder();
            _userBuilder = new UserTestBuilder();
            _request = _builder.BuildUpdate();
            _userAdmin = _userBuilder.DbBuild();
            _userAdmin.Id = _idAdmin;
            _listUser = [_userAdmin];
            _document = _builder.DbBuild();
            _document.DocumentType.Id = (int)FileTypes.Text;
            _document.IdDocumentType = (int)FileTypes.Text;
            _document.IdUser = _idAdmin;
            _document.Users.Id = _idAdmin;
        }

        [Test]
        public async Task Given_ValidPayload_With_ValidUpdateDocument_WhenInvokeValidator_Then_ItShouldPassValidation()
        {

            _documentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_document);

            _userRepository.Setup(x => x.IdExistsAsync(_request.ModifyUser)).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            await _sut.ValidateAndThrowAsync(_request);

            ClassicAssert.Pass();
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyId_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {

            _documentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_document);

            _userRepository.Setup(x => x.IdExistsAsync(_request.ModifyUser)).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = DocumentExceptions.RequiredId;

            _request.Id = Guid.Empty;

            CaptureExceptionAndValidate(exceptionMessage);

        }

        [Test]
        public void Given_InvalidPayload_With_NotExistDocument_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {

            _documentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_document);

            _userRepository.Setup(x => x.IdExistsAsync(_request.ModifyUser)).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = DocumentExceptions.DocumentNotExist;

            CaptureExceptionAndValidate(exceptionMessage);

        }

        [Test]
        public void Given_InvalidPayload_With_DocumentNotTypeText_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _documentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _document.DocumentType.Id = (int)FileTypes.Document;
            _document.IdDocumentType = (int)FileTypes.Document;

            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_document);

            _userRepository.Setup(x => x.IdExistsAsync(_request.ModifyUser)).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = DocumentExceptions.DocumentNotMatch;

            CaptureExceptionAndValidate(exceptionMessage);

        }

        [Test]
        public void Given_InvalidPayload_With_EmptyName_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {

            _documentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_document);

            _userRepository.Setup(x => x.IdExistsAsync(_request.ModifyUser)).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = DocumentExceptions.RequiredName;

            _request.Name = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyDescription_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {

            _documentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_document);

            _userRepository.Setup(x => x.IdExistsAsync(_request.ModifyUser)).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = DocumentExceptions.RequiredDescription;

            _request.Description = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }


        [Test]
        public void Given_InvalidPayload_With_EmptyModifyUser_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {

            _documentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_document);

            _userRepository.Setup(x => x.IdExistsAsync(_request.ModifyUser)).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = DocumentExceptions.CreateUserNotExist;

            _request.ModifyUser = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_NotExistModifyUser_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {

            _documentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_document);

            _userRepository.Setup(x => x.IdExistsAsync(_request.ModifyUser)).ReturnsAsync(false);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = DocumentExceptions.UserNotExist;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_NoAdminUserModify_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {

            _documentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            _document.IdUser = Guid.NewGuid();
            _document.Users.Id = _document.IdUser;

            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_document);

            _userRepository.Setup(x => x.IdExistsAsync(_request.ModifyUser)).ReturnsAsync(false);

            _documentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _listUser.Clear();
            var user = _userBuilder.DbBuild();

            user.IdUserType = 2;
            user.UserTypes.Id = 2;
            user.Id = Guid.NewGuid();

            _listUser = [user];

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            var exceptionMessage = DocumentExceptions.CreateUserNotExist;

            CaptureExceptionAndValidate(exceptionMessage);

        }

        private void CaptureExceptionAndValidate(string exceptionMessage)
        {
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.ValidateAndThrowAsync(_request));
            ClassicAssert.That(exceptionReceived.Message.Contains(exceptionMessage));
        }
    }
}

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
    public class DeleteCommetRequestValidatorTest
    {
        private IValidator<DeleteCommetRequest> _sut;
        private DeleteCommetRequest _request;
        private CommentTestBuilder _builder;
        private UserTestBuilder _userBuilder;
        private Mock<IUserRepository> _userRepository;
        private Mock<ICommentRepository> _commentRepository;
        private Mock<IConfigurationRoot> _configurationRoot;
        private Users _userAdmin;
        private List<Users> _listUser;
        private Comments _comment;
        private readonly Guid _idAdmin = Guid.Parse("679bd613-da71-48b9-bf5c-b7b598935b77");

        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();
            _commentRepository = new Mock<ICommentRepository>();
            _configurationRoot = new Mock<IConfigurationRoot>();
            _configurationRoot.Setup(x => x["IdUserAdmin"]).Returns(_idAdmin.ToString());
            _sut = new DeleteCommetRequestValidator(_configurationRoot.Object, _userRepository.Object, _commentRepository.Object);
            _builder = new CommentTestBuilder();
            _userBuilder = new UserTestBuilder();
            _request = _builder.DeleteBuild();
            _userAdmin = _userBuilder.DbBuild();
            _userAdmin.Id = _idAdmin;
            _listUser = [_userAdmin];
            _comment = _builder.DbBuild();
            _comment.IdUser = _idAdmin;
            _comment.Users.Id = _idAdmin;
        }

        [Test]
        public async Task Given_ValidPayload_With_ValidDeleteComment_WhenInvokeValidator_Then_ItShouldPassValidation()
        {
            _commentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _userRepository.Setup(x => x.IdExistsAsync(_request.ModifyUser)).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            _commentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_comment);

            await _sut.ValidateAndThrowAsync(_request);
            ClassicAssert.Pass();
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyId_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _commentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _userRepository.Setup(x => x.IdExistsAsync(_request.ModifyUser)).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            _commentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_comment);

            var exceptionMessage = CommentExceptions.RequiredId;

            _request.Id = Guid.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_EmptyModidyUser_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _commentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _userRepository.Setup(x => x.IdExistsAsync(_request.ModifyUser)).ReturnsAsync(true);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            _commentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_comment);

            var exceptionMessage = CommentExceptions.CreateUserNotExist;

            _request.ModifyUser = string.Empty;

            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_NotExistModidyUser_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _commentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _userRepository.Setup(x => x.IdExistsAsync(_request.ModifyUser)).ReturnsAsync(false);

            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            _commentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_comment);

            var exceptionMessage = CommentExceptions.CreateUserNotExist;


            CaptureExceptionAndValidate(exceptionMessage);
        }

        [Test]
        public void Given_InvalidPayload_With_NoPermission_and_Allowed_WhenInvokeValidator_Then_ItShouldNotPassValidation()
        {
            _commentRepository.Setup(x => x.IdExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _userRepository.Setup(x => x.IdExistsAsync(_request.ModifyUser)).ReturnsAsync(true);

            _listUser.Clear();
            var user = _userBuilder.DbBuild();
            user.Id = Guid.NewGuid();
            _listUser = [user];
            _userRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Users, bool>>>())).ReturnsAsync(_listUser);

            _comment.Users.Id = Guid.NewGuid();
            _comment.IdUser = _comment.Users.Id;
            _commentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_comment);

            var exceptionMessage = CommentExceptions.CreateUserNotExist;


            CaptureExceptionAndValidate(exceptionMessage);
        }

        private void CaptureExceptionAndValidate(string exceptionMessage)
        {
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.ValidateAndThrowAsync(_request));
            ClassicAssert.That(exceptionReceived.Message.Contains(exceptionMessage));
        }
    }
}

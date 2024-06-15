using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Schema.DB;
using Bridgenext.Test.Builders;
using NUnit.Framework.Legacy;
using Bridgenext.DataAccess.DTOAdapter;

namespace Bridgenext.Test.UnitTest.DataAccess
{
    [TestFixture]
    public class UserAdapterTest
    {
        private CreateUserRequest _createUserRequest;
        private UpdateUserRequest _updateUserRequest;
        private DeleteUserRequest _deleteUserRequest;
        private Users _dbUser;
        private UserTestBuilder _userTestBuilder;

        [SetUp]
        public void SetUp()
        {
            _userTestBuilder = new UserTestBuilder();
            _dbUser = _userTestBuilder.DbBuild();
            _createUserRequest = _userTestBuilder.CreateBuilder();
            _updateUserRequest = _userTestBuilder.UpdateBuilder(); 
            _deleteUserRequest = _userTestBuilder.DeleteBuilder();
        }

        [Test]
        public void Given_A_UserDatabaseModel_When_IInvokeAdapter_Then_IShould_ReceiveTheDomainModelCorrectly()
        {
            var _domainModel = _dbUser.ToDomainModel();

            ClassicAssert.That(_dbUser.Id == _domainModel.Id);
        }

        [Test]
        public void Given_A_UserDatabaseModelNull_When_IInvokeAdapter_Then_IShould_ReceiveTheDomainModelNull()
        {
            _dbUser = null;
            var _domainModel = _dbUser.ToDomainModel();

            ClassicAssert.IsNull(_domainModel);
        }

        [Test]
        public void Given_Two_UserDatabaseModel_When_IInvokeAdapter_Then_IShould_ReceiveTheDomainModelCorrectly()
        {
            List<Users> listUser = [_userTestBuilder.DbBuild(), _userTestBuilder.DbBuild()];

            var _domainModel = listUser.ToDomainModel();

            ClassicAssert.That(listUser.Count() == _domainModel.Count());
        }

        [Test]
        public void Given_A_CreateUserModel_When_IInvokeAdapter_Then_IShould_ReceiveTheDomainDataBaseCorrectly()
        {
            var _dbModel = _createUserRequest.ToDatabaseModel();

            ClassicAssert.That(_createUserRequest.Email == _dbModel.Email);
        }

        [Test]
        public void Given_A_CreateUserModelNull_When_IInvokeAdapter_Then_IShould_ReceiveTheDataBaseModelNull()
        {
            _createUserRequest = null;
            var _dbModel = _createUserRequest.ToDatabaseModel();

            ClassicAssert.IsNull(_dbModel);
        }

        [Test]
        public void Given_A_UpdateUserModel_When_IInvokeAdapter_Then_IShould_ReceiveTheDomainDataBaseCorrectly()
        {
            var _dbModel = _updateUserRequest.ToDatabaseModel(_dbUser);

            ClassicAssert.That(_updateUserRequest.Email == _dbModel.Email);
            ClassicAssert.That(_dbUser.Id == _dbModel.Id);
        }

        [Test]
        public void Given_A_UpdateUserModelNull_When_IInvokeAdapter_Then_IShould_ReceiveTheDataBaseModelNull()
        {
            _updateUserRequest = null;
            var _dbModel = _updateUserRequest.ToDatabaseModel(_dbUser);

            ClassicAssert.IsNull(_dbModel);
        }

        [Test]
        public void Given_A_UpdateUserModelNull2_When_IInvokeAdapter_Then_IShould_ReceiveTheDataBaseModelNull()
        {
            var _dbModel = _updateUserRequest.ToDatabaseModel(null);

            ClassicAssert.IsNull(_dbModel);
        }

        [Test]
        public void Given_A_UpdateUserModelNull3_When_IInvokeAdapter_Then_IShould_ReceiveTheDataBaseModelNull()
        {
            _updateUserRequest = null;
            var _dbModel = _updateUserRequest.ToDatabaseModel(null);

            ClassicAssert.IsNull(_dbModel);
        }

        [Test]
        public void Given_A_DeleteUserModel_When_IInvokeAdapter_Then_IShould_ReceiveTheDomainDataBaseCorrectly()
        {
            var _dbModel = _deleteUserRequest.ToDatabaseModel(_dbUser);

            ClassicAssert.That(_deleteUserRequest.ModifyUser == _dbModel.ModifyUser);
            ClassicAssert.That(_dbUser.Id == _dbModel.Id);
        }

        [Test]
        public void Given_A_DeleteUserModelNull_When_IInvokeAdapter_Then_IShould_ReceiveTheDataBaseModelNull()
        {
            _deleteUserRequest = null;
            var _dbModel = _deleteUserRequest.ToDatabaseModel(_dbUser);

            ClassicAssert.IsNull(_dbModel);
        }

        [Test]
        public void Given_A_DeleteUserModelNull2_When_IInvokeAdapter_Then_IShould_ReceiveTheDataBaseModelNull()
        {
            var _dbModel = _deleteUserRequest.ToDatabaseModel(null);

            ClassicAssert.IsNull(_dbModel);
        }

        [Test]
        public void Given_A_DeleteUserModelNull3_When_IInvokeAdapter_Then_IShould_ReceiveTheDataBaseModelNull()
        {
            _deleteUserRequest = null;
            var _dbModel = _deleteUserRequest.ToDatabaseModel(null);

            ClassicAssert.IsNull(_dbModel);
        }

    }
}

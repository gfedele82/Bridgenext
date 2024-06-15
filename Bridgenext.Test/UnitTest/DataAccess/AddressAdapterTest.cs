using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Schema.DB;
using Bridgenext.Test.Builders;
using NUnit.Framework.Legacy;
using Bridgenext.DataAccess.DTOAdapter;

namespace Bridgenext.Test.UnitTest.DataAccess
{
    [TestFixture]
    public class AddressAdapterTest
    {
        private CreateAddressRequest _createAddressRequest;
        private UpdateAddressRequest _updateAddressRequest;
        private Addreesses _dbAdrress;
        private AddressTestBuilder _addressTestBuilder;

        [SetUp]
        public void SetUp()
        {
            _addressTestBuilder = new AddressTestBuilder();
            _dbAdrress = _addressTestBuilder.DbBuild();
            _createAddressRequest = _addressTestBuilder.CreateBuilder();
            _updateAddressRequest = _addressTestBuilder.UpdateBuilder();
        }

        [Test]
        public void Given_A_AddressDatabaseModel_When_IInvokeAdapter_Then_IShould_ReceiveTheDomainModelCorrectly()
        {
            var _domainModel = _dbAdrress.ToDomainModel();

            ClassicAssert.That(_dbAdrress.Id == _domainModel.Id);
        }

        [Test]
        public void Given_A_AddressDatabaseModelNull_When_IInvokeAdapter_Then_IShould_ReceiveTheDomainModelNull()
        {
            _dbAdrress = null;
            var _domainModel = _dbAdrress.ToDomainModel();

            ClassicAssert.IsNull(_domainModel);
        }

        [Test]
        public void Given_Two_AddressDatabaseModel_When_IInvokeAdapter_Then_IShould_ReceiveTheDomainModelCorrectly()
        {
            List<Addreesses> listAddress = [_addressTestBuilder.DbBuild(), _addressTestBuilder.DbBuild()];

            var _domainModel = listAddress.ToDomainModel();

            ClassicAssert.That(listAddress.Count() == _domainModel.Count());
        }

        [Test]
        public void Given_A_CreateAddressModel_When_IInvokeAdapter_Then_IShould_ReceiveTheDomainDataBaseCorrectly()
        {
            var userId = Guid.NewGuid();
            var _dbModel = _createAddressRequest.ToDatabaseModel(userId);

            ClassicAssert.That(_createAddressRequest.City == _dbModel.City);
            ClassicAssert.That(userId == _dbModel.IdUser);
        }

        [Test]
        public void Given_A_CreateAddressModelNull_When_IInvokeAdapter_Then_IShould_ReceiveTheDataBaseModelNull()
        {
            var userId = Guid.NewGuid();
            _createAddressRequest = null;
            var _dbModel = _createAddressRequest.ToDatabaseModel(userId);

            ClassicAssert.IsNull(_dbModel);
        }

        [Test]
        public void Given_Two_CreateAddressModel_When_IInvokeAdapter_Then_IShould_ReceiveTheDataBaseModelCorrectly()
        {
            var userId = Guid.NewGuid();
            List<CreateAddressRequest> listAddress = [_addressTestBuilder.CreateBuilder(), _addressTestBuilder.CreateBuilder()];

            var _dbModel = listAddress.ToDatabaseModel(userId);

            ClassicAssert.That(listAddress.Count() == _dbModel.Count());
        }

        [Test]
        public void Given_A_UpdateAddressModel_When_IInvokeAdapter_Then_IShould_ReceiveTheDomainDataBaseCorrectly()
        {
            var userId = Guid.NewGuid();
            var _dbModel = _updateAddressRequest.ToDatabaseModel(userId);

            ClassicAssert.That(_updateAddressRequest.City == _dbModel.City);
            ClassicAssert.That(userId == _dbModel.IdUser);
        }

        [Test]
        public void Given_A_UpdateAddressModelNull_When_IInvokeAdapter_Then_IShould_ReceiveTheDataBaseModelNull()
        {
            var userId = Guid.NewGuid();
            _updateAddressRequest = null;
            var _dbModel = _updateAddressRequest.ToDatabaseModel(userId);

            ClassicAssert.IsNull(_dbModel);
        }
    }
}

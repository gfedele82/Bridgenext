using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Schema.DB;
using Bridgenext.Test.Builders;
using NUnit.Framework.Legacy;
using Bridgenext.DataAccess.DTOAdapter;
using ZstdSharp.Unsafe;

namespace Bridgenext.Test.UnitTest.DataAccess
{
    [TestFixture]
    public class DocumentAdapterTest
    {
        private DocumentTestBuilder _documentTestBuilder;
        private Documents _dbDocument;
        private DisableDocumentRequest _disableDocumentRequest;
        private UpdateDocumentRequest _updateDocumentRequest;

        [SetUp]
        public void SetUp()
        {
            _documentTestBuilder = new DocumentTestBuilder();
            _dbDocument = _documentTestBuilder.DbBuild();
            _disableDocumentRequest = _documentTestBuilder.BuildDisable();
            _updateDocumentRequest = _documentTestBuilder.BuildUpdate();
        }

        [Test]
        public void Given_A_DocumentDatabaseModel_When_IInvokeAdapter_Then_IShould_ReceiveTheDomainModelCorrectly()
        {
            var _domainModel = _dbDocument.ToDomainModel();

            ClassicAssert.That(_dbDocument.Id == _domainModel.Id);
        }

        [Test]
        public void Given_A_DocumentDatabaseModelNull_When_IInvokeAdapter_Then_IShould_ReceiveTheDomainModelNull()
        {
            _dbDocument = null;
            var _domainModel = _dbDocument.ToDomainModel();

            ClassicAssert.IsNull(_domainModel);
        }

        [Test]
        public void Given_Two_DocumentDatabaseModel_When_IInvokeAdapter_Then_IShould_ReceiveTheDomainModelCorrectly()
        {
            List<Documents> listDocument = [_documentTestBuilder.DbBuild(), _documentTestBuilder.DbBuild()];

            var _domainModel = listDocument.ToDomainSearchModel();

            ClassicAssert.That(listDocument.Count() == _domainModel.Count());
        }

        [Test]
        public void Given_A_DocumentDatabaseModel_When_IInvokeAdapter_Then_IShould_ReceiveTheSearchDomainModelCorrectly()
        {
            var _domainModel = _dbDocument.ToDomainSearchModel();

            ClassicAssert.That(_dbDocument.Id == _domainModel.Id);
        }

        [Test]
        public void Given_A_DocumentDatabaseModelNull_When_IInvokeAdapter_Then_IShould_ReceiveTheSearchDomainModelNull()
        {
            _dbDocument = null;
            var _domainModel = _dbDocument.ToDomainSearchModel();

            ClassicAssert.IsNull(_domainModel);
        }

        [Test]
        public void Given_Two_DocumentDatabaseModel_When_IInvokeAdapter_Then_IShould_ReceiveTheSearchDomainModelCorrectly()
        {
            List<Documents> listDocument = [_documentTestBuilder.DbBuild(), _documentTestBuilder.DbBuild()];

            var _domainModel = listDocument.ToDomainModel();

            ClassicAssert.That(listDocument.Count() == _domainModel.Count());
        }

        [Test]
        public void Given_A_DisableDocumentModel_When_IInvokeAdapter_Then_IShould_ReceiveTheDomainDataBaseCorrectly()
        {
            var dbDoc = _documentTestBuilder.DbBuild();
            _disableDocumentRequest.Id = dbDoc.Id;
            var _dbModel = _disableDocumentRequest.ToDatabaseModel(dbDoc);

            ClassicAssert.That(_disableDocumentRequest.Id == _dbModel.Id);
            ClassicAssert.IsTrue(_dbModel.Hide);
        }

        [Test]
        public void Given_A_DisableDocumentModelNull_When_IInvokeAdapter_Then_IShould_ReceiveTheDataBaseModelNull()
        {
            _disableDocumentRequest = null;
            var _dbModel = _disableDocumentRequest.ToDatabaseModel(_dbDocument);

            ClassicAssert.IsNull(_dbModel);
        }

        [Test]
        public void Given_A_DisableDocumentModelNull2_When_IInvokeAdapter_Then_IShould_ReceiveTheDataBaseModelNull()
        {
            var _dbModel = _disableDocumentRequest.ToDatabaseModel(null);

            ClassicAssert.IsNull(_dbModel);
        }

        [Test]
        public void Given_A_DisableDocumentModelNull3_When_IInvokeAdapter_Then_IShould_ReceiveTheDataBaseModelNull()
        {
            _disableDocumentRequest = null;
            var _dbModel = _disableDocumentRequest.ToDatabaseModel(_dbDocument);

            ClassicAssert.IsNull(_dbModel);
        }

        [Test]
        public void Given_A_UpdateDocumentModel_When_IInvokeAdapter_Then_IShould_ReceiveTheDomainDataBaseCorrectly()
        {
            var dbDoc = _documentTestBuilder.DbBuild();
            _updateDocumentRequest.Id = dbDoc.Id;
            var _dbModel = _updateDocumentRequest.ToDatabaseModel(dbDoc);

            ClassicAssert.That(_updateDocumentRequest.Id == _dbModel.Id);
            ClassicAssert.That(_updateDocumentRequest.Description == _dbModel.Description);
        }

        [Test]
        public void Given_A_UpdateDocumentModelNull_When_IInvokeAdapter_Then_IShould_ReceiveTheDataBaseModelNull()
        {
            _updateDocumentRequest = null;
            var _dbModel = _updateDocumentRequest.ToDatabaseModel(_dbDocument);

            ClassicAssert.IsNull(_dbModel);
        }

        [Test]
        public void Given_A_UpdateDocumentModelNull2_When_IInvokeAdapter_Then_IShould_ReceiveTheDataBaseModelNull()
        {
            var _dbModel = _updateDocumentRequest.ToDatabaseModel(null);

            ClassicAssert.IsNull(_dbModel);
        }

        [Test]
        public void Given_A_UpdateDocumentModelNull3_When_IInvokeAdapter_Then_IShould_ReceiveTheDataBaseModelNull()
        {
            _updateDocumentRequest = null;
            var _dbModel = _updateDocumentRequest.ToDatabaseModel(_dbDocument);

            ClassicAssert.IsNull(_dbModel);
        }
    }
}

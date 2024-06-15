using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Schema.DB;
using Bridgenext.Test.Builders;
using NUnit.Framework.Legacy;
using Bridgenext.DataAccess.DTOAdapter;

namespace Bridgenext.Test.UnitTest.DataAccess
{
    public class CommentAdapterTest
    {
        private CreateCommetRequest _createCommentRequest;
        private Comments _dbComment;
        private CommentTestBuilder _commentTestBuilder;

        [SetUp]
        public void SetUp()
        {
            _commentTestBuilder = new CommentTestBuilder();
            _dbComment = _commentTestBuilder.DbBuild();
            _createCommentRequest = _commentTestBuilder.CreateBuild();
        }

        [Test]
        public void Given_A_CommentDatabaseModel_When_IInvokeAdapter_Then_IShould_ReceiveTheDomainModelCorrectly()
        {
            var _domainModel = _dbComment.ToDomainModel();

            ClassicAssert.That(_dbComment.Id == _domainModel.Id);
        }

        [Test]
        public void Given_A_CommentDatabaseModelNull_When_IInvokeAdapter_Then_IShould_ReceiveTheDomainModelNull()
        {
            _dbComment = null;
            var _domainModel = _dbComment.ToDomainModel();

            ClassicAssert.IsNull(_domainModel);
        }

        [Test]
        public void Given_Two_CommentDatabaseModel_When_IInvokeAdapter_Then_IShould_ReceiveTheDomainModelCorrectly()
        {
            List<Comments> listComment = [_commentTestBuilder.DbBuild(), _commentTestBuilder.DbBuild()];

            var _domainModel = listComment.ToDomainModel();

            ClassicAssert.That(listComment.Count() == _domainModel.Count());
        }

        [Test]
        public void Given_A_CreateCommentModel_When_IInvokeAdapter_Then_IShould_ReceiveTheDomainDataBaseCorrectly()
        {
            var userTestBuilder = new UserTestBuilder();
            var documentTestBuilder = new DocumentTestBuilder();

            var user = userTestBuilder.DbBuild();
            var document = documentTestBuilder.DbBuild();
            document.Id = _createCommentRequest.IdDocument;

            var _dbModel = _createCommentRequest.ToDatabaseModel(document, user);

            ClassicAssert.That(_createCommentRequest.Content == _dbModel.Comment);
            ClassicAssert.That(document.Id == _dbModel.IdDocumnet);
            ClassicAssert.That(user.Id == _dbModel.IdUser);
        }

        [Test]
        public void Given_A_CreateCommentModelNull_When_IInvokeAdapter_Then_IShould_ReceiveTheDataBaseModelNull()
        {
            var userTestBuilder = new UserTestBuilder();
            var documentTestBuilder = new DocumentTestBuilder();

            var user = userTestBuilder.DbBuild();
            var document = documentTestBuilder.DbBuild();
            _createCommentRequest = null;
            var _dbModel = _createCommentRequest.ToDatabaseModel(document, user);

            ClassicAssert.IsNull(_dbModel);
        }

        [Test]
        public void Given_A_CreateCommentModelNull2_When_IInvokeAdapter_Then_IShould_ReceiveTheDataBaseModelNull()
        {
            var documentTestBuilder = new DocumentTestBuilder();

            var document = documentTestBuilder.DbBuild();
            var _dbModel = _createCommentRequest.ToDatabaseModel(document, null);

            ClassicAssert.IsNull(_dbModel);
        }

        [Test]
        public void Given_A_CreateCommentModelNull3_When_IInvokeAdapter_Then_IShould_ReceiveTheDataBaseModelNull()
        {
            var userTestBuilder = new UserTestBuilder();

            var user = userTestBuilder.DbBuild();

            var _dbModel = _createCommentRequest.ToDatabaseModel(null, user);

            ClassicAssert.IsNull(_dbModel);
        }

        [Test]
        public void Given_A_CreateCommentModelNull4_When_IInvokeAdapter_Then_IShould_ReceiveTheDataBaseModelNull()
        {
            var _dbModel = _createCommentRequest.ToDatabaseModel(null, null);

            ClassicAssert.IsNull(_dbModel);
        }

        [Test]
        public void Given_A_CreateCommentModelNull5_When_IInvokeAdapter_Then_IShould_ReceiveTheDataBaseModelNull()
        {
            _createCommentRequest = null;

            var _dbModel = _createCommentRequest.ToDatabaseModel(null, null);

            ClassicAssert.IsNull(_dbModel);
        }
    }
}

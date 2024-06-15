using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Schema.DB;

namespace Bridgenext.Test.Builders
{
    public class CommentTestBuilder
    {
        private readonly Comments _dbComment;
        private readonly CreateCommetRequest _createComment;
        private readonly DeleteCommetRequest _deleteComment;
        private readonly string _adminUser = "admin@admin.admin";
        private readonly DocumentTestBuilder _documentTestBuilder;
        private readonly UserTestBuilder _userTestBuilder;

        public CommentTestBuilder()
        {
            _documentTestBuilder = new DocumentTestBuilder();
            var document = _documentTestBuilder.DbBuild();

            _userTestBuilder = new UserTestBuilder();
            var user = _userTestBuilder.DbBuild();

            _dbComment = new Comments()
            {
                Comment = "test comment",
                Date = DateTime.Now,
                Documents = document,
                Id = Guid.NewGuid(),
                IdDocumnet = document.Id,
                IdUser = user.Id,
                Users = user
            };

            _createComment = new CreateCommetRequest()
            {
                Content = "test content",
                CreateUser = _adminUser,
                IdDocument = Guid.NewGuid()
            };

            _deleteComment = new DeleteCommetRequest()
            {
                Id = Guid.NewGuid(),
                ModifyUser = _adminUser
            };

        }

        public Comments DbBuild() => _dbComment;

        public CreateCommetRequest CreateBuild() => _createComment;

        public DeleteCommetRequest DeleteBuild() => _deleteComment;
    }
}

using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.DTO.Response;
using Bridgenext.Models.Schema.DB;

namespace Bridgenext.DataAccess.DTOAdapter
{
    public static class CommentAdapter
    {
        public static Comments ToDatabaseModel(this CreateCommetRequest commentRequest, Documents document, Users user)
        {
            if (commentRequest == null)
            {
                return null;
            }

            return new Comments()
            {
                Comment = commentRequest.Content,
                Date = DateTime.Now,
                Id = Guid.NewGuid(),
                IdDocumnet = commentRequest.IdDocument,
                Documents = document,
                Users = user,
                IdUser = user.Id
            };

        }

        public static Comments ToDatabaseModel(this DisableDocumentRequest documentRequest, Documents document, Users user)
        {
            if (documentRequest == null)
            {
                return null;
            }

            return new Comments()
            {
                Comment = documentRequest.Comment,
                Date = DateTime.Now,
                Id = Guid.NewGuid(),
                IdDocumnet = documentRequest.Id,
                Documents = document,
                Users = user,
                IdUser = user.Id
            };

        }

        public static CommentDto ToDomainModel(this Comments dbComment)
        {
            if (dbComment == null)
            {
                return null;
            }

            return new CommentDto()
            {
                Id = dbComment.Id,
                IdDoc = dbComment.IdDocumnet,
                CreateDate = dbComment.Date,
                CreateUser = dbComment.Users.Email

            };
        }

        public static IEnumerable<CommentDto> ToDomainModel(this IEnumerable<Comments> dbComments)
        {
            foreach (var comment in dbComments)
            {
                yield return comment.ToDomainModel();
            }
        }
    }
}

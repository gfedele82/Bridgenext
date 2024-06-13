using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Schema.DB;

namespace Bridgenext.DataAccess.DTOAdapter
{
    public static class CommentAdapter
    {
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
    }
}

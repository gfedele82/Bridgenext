using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.DTO.Response;
using Bridgenext.Models.Schema.DB;

namespace Bridgenext.DataAccess.DTOAdapter
{
    public static class DocumentAdapter
    {
        public static DocumentDto ToDomainModel(this Documents dbDocument)
        {
            if (dbDocument == null)
            {
                return null;
            }

            return new DocumentDto()
            {
                Id = dbDocument.Id,
                CreateDate = dbDocument.CreateDate,
                CreateUser = dbDocument.CreateUser,
                ModifyDate = dbDocument.ModifyDate,
                ModifyUser = dbDocument.ModifyUser,
                Content = dbDocument.Content,
                Description = dbDocument.Description,
                DocumentType = new DocumentTypeDto()
                {
                    Id = dbDocument.DocumentType.Id,
                    Type = dbDocument.DocumentType.Type
                },
                 FileName = dbDocument.FileName,
                 Hide = dbDocument.Hide,
                 Name = dbDocument.Name,
                 Size = dbDocument.Size,
                 SourceFile = dbDocument.SourceFile,
                 TargetFile = dbDocument.TargetFile,
                 Users = new UserDto()
                 {
                     CreateDate = dbDocument.Users.CreateDate,
                     CreateUser = dbDocument.Users.CreateUser,
                     Email = dbDocument.Users.Email,
                     FirstName = dbDocument.Users.FirstName,
                     LastName = dbDocument.Users.LastName,
                     Id = dbDocument.Users.Id, 
                     ModifyDate = dbDocument.Users.ModifyDate,
                     ModifyUser= dbDocument.Users.ModifyUser,
                     UserType = new UserTypeDto()
                     {
                        Id = dbDocument.Users.UserTypes.Id,
                        Type = dbDocument.Users.UserTypes.Type
                     }
                 }
            };
        }

        public static IEnumerable<DocumentDto> ToDomainModel(this IEnumerable<Documents> dbDocuments)
        {
            foreach (var document in dbDocuments)
            {
                yield return document.ToDomainModel();
            }
        }

        public static Documents ToDatabaseModel(this DisableDocumentRequest documentRequest, Documents existDocument)
        {
            if (documentRequest == null || existDocument == null)
            {
                return null;
            }

            existDocument.Hide = true;
            existDocument.ModifyDate = DateTime.Now;
            existDocument.ModifyUser = documentRequest.ModifyUser;

            return existDocument;

        }

        public static Documents ToDatabaseModel(this UpdateDocumentRequest documentRequest, Documents existDocument)
        {
            if (documentRequest == null || existDocument == null)
            {
                return null;
            }

            existDocument.Name = documentRequest.Name;
            existDocument.Description = documentRequest.Description;
            existDocument.Content = documentRequest.Content;
            existDocument.ModifyDate = DateTime.Now;
            existDocument.ModifyUser = documentRequest.ModifyUser;

            return existDocument;

        }

        public static DocumentSearchDto ToDomainSearchModel (this Documents dbDocument)
        {
            if (dbDocument == null)
                return null;

            return new DocumentSearchDto()
            {
                Id = dbDocument.Id,
                Content = dbDocument.Content,
                CreateDate = dbDocument.CreateDate,
                CreateUser = dbDocument.Users.CreateUser,
                Description = dbDocument.Description,
                ModifyDate = dbDocument.Users.ModifyDate,
                ModifyUser = dbDocument.Users.ModifyUser,
                Name = dbDocument.Name
            };

        }

        public static IEnumerable<DocumentSearchDto> ToDomainSearchModel(this IEnumerable<Documents> dbDocuments)
        {
            foreach (var document in dbDocuments)
            {
                yield return document.ToDomainSearchModel();
            }
        }
    }
}

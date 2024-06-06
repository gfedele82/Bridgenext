using Bridgenext.Models.DTO.Response;
using Bridgenext.Models.Schema;

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
    }
}

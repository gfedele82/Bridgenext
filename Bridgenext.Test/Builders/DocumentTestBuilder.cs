using Bogus;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Enums;
using Bridgenext.Models.Schema.DB;
using Bridgenext.Models.Schema.NotSQL;
using MongoDB.Bson;

namespace Bridgenext.Test.Builders
{
    public class DocumentTestBuilder
    {
        private CreateDocumentRequest _createDocumentRequest;
        private UpdateDocumentFileRequest _updateDocumentFileRequest;
        private DeleteDocumentRequest _deleteDocumentRequest;
        private UpdateDocumentRequest _updateDocumentRequest;
        private DisableDocumentRequest _disableDocumentRequest;
        private Documents _dbDocument;
        private MongoDocuments _dbMongoDocument;
        private readonly string _adminUser = "admin@admin.admin";

        public DocumentTestBuilder()
        {
            Faker faker = new("en_US");

            _createDocumentRequest = new CreateDocumentRequest()
            {
                Content = "Test content",
                CreateUser = _adminUser,
                Description = "Description",
                File = "test.txt",
                Name = "Name"
            };

            _updateDocumentFileRequest = new UpdateDocumentFileRequest()
            {
                File = "test.txt",
                Id = Guid.NewGuid(),
                ModifyUser = _adminUser,
                Name = "Name",
                Description = "Description"
            };

            _deleteDocumentRequest = new DeleteDocumentRequest()
            {
                ModifyUser = _adminUser,
                Id = Guid.NewGuid()
            };

            _updateDocumentRequest = new UpdateDocumentRequest()
            {
                Content = "Test content",
                ModifyUser = _adminUser,
                Description = "Description",
                Name = "Name",
                Id = Guid.NewGuid()
            };

            _disableDocumentRequest = new DisableDocumentRequest()
            {
                Id = Guid.NewGuid(),
                Comment = "comment test",
                ModifyUser = _adminUser
            };

            Guid idUser = Guid.NewGuid();
            _dbDocument = new Documents()
            {
                Content = "Test content",
                CreateUser = _adminUser,
                Description = "Description",
                SourceFile = "test.txt",
                TargetFile = "test.txt",
                Name = "Name",
                Hide = faker.Random.Bool(),
                FileName = "test.txt",
                CreateDate = DateTime.Now,
                DocumentType = new DocumentsType()
                {
                    Id = (int)FileTypes.Document,
                    Type = Enum.GetName(typeof(UsersTypeEnum), (int)UsersTypeEnum.Administrator)
                },
                IdDocumentType = (int)FileTypes.Document,
                Id = Guid.NewGuid(),
                ModifyDate = DateTime.Now,
                ModifyUser = _adminUser,
                Size = faker.Random.Int(),
                IdUser = idUser,
                Users = new Users()
                {
                    Id = idUser,
                    Email = _adminUser
                }
            };

            _dbMongoDocument = new MongoDocuments()
            {
                content = "test",
                CreateUser = "test",
                IdDb = Guid.NewGuid().ToString(),
                uploadDate = DateTime.Now
            };
        }

        public CreateDocumentRequest BuildCreate() => _createDocumentRequest;

        public UpdateDocumentFileRequest BuildUpdateFile() => _updateDocumentFileRequest;

        public DeleteDocumentRequest BuildDelete() => _deleteDocumentRequest;

        public UpdateDocumentRequest BuildUpdate() => _updateDocumentRequest;

        public DisableDocumentRequest BuildDisable() => _disableDocumentRequest;

        public Documents DbBuild() => _dbDocument;

        public MongoDocuments DbMongoBuild() => _dbMongoDocument;

    }
}

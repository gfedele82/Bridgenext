using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Engine;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Engine.Utils;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Bridgenext.Test.Builders;
using NUnit.Framework.Legacy;
using Bridgenext.Models.Schema.DB;
using Bridgenext.Engine.Interfaces;
using Bridgenext.Models.Enums;
using Bridgenext.Models.DTO;
using System.Linq.Expressions;
using Bridgenext.Models.Schema.NotSQL;

namespace Bridgenext.Test.UnitTest.Engines
{
    [TestFixture]
    public class DocumentEngineTest
    {
        Mock<ILogger<DocumentEngine>> _logger;
        Mock<DocumentTypeResolver> _documentTypeResolver;
        IConfigurationRoot _configuration;
        Mock<IUserRepository> _userRepository;
        Mock<IMongoRepostory> _mongoRepository;
        Mock<ICommentRepository> _commentRepository;
        Mock<IDocumentRepositoty> _documentRepository;
        Mock<IValidator<CreateDocumentRequest>> _addDocumentRequestValidator;
        Mock<IValidator<Guid>> _downloadRequestValidator;
        Mock<IValidator<DisableDocumentRequest>> _disabledocumentValidator;
        Mock<IValidator<UpdateDocumentRequest>> _updateDocumentValidator;
        Mock<IValidator<UpdateDocumentFileRequest>> _updateDocumentFileValidator;
        Mock<IValidator<DeleteDocumentRequest>> _deleteDocumentValidator;
        DocumentEngine _sut;
        CreateDocumentRequest _createDocumentRequest;
        DisableDocumentRequest _disableDocumentRequest;
        UpdateDocumentRequest _updateDocumentRequest;
        UpdateDocumentFileRequest _updateDocumentFileRequest;
        DeleteDocumentRequest _deleteDocumentRequest;
        UserTestBuilder _userTestBuilder;
        DocumentTestBuilder _documentTestBuilder;
        CommentTestBuilder _commentTestBuilder;
        Mock<IProcessDocumentByType>? _mockDocumentText;
        Mock<IProcessDocumentByType>? _mockDocumentVideo;
        Mock<IProcessDocumentByType>? _mockDocumentImage;
        Mock<IProcessDocumentByType>? _mockDocumentDocument;


        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<DocumentEngine>>();
            _documentTypeResolver = new();
            _mockDocumentDocument = new();
            _mockDocumentVideo = new();
            _mockDocumentImage = new();
            _mockDocumentText = new();
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", true, true)
                .Build();
                
                
            _userRepository = new Mock<IUserRepository>();
            _mongoRepository = new Mock<IMongoRepostory>();
            _commentRepository = new Mock<ICommentRepository>();
            _documentRepository = new Mock<IDocumentRepositoty>();
            _addDocumentRequestValidator = new Mock<IValidator<CreateDocumentRequest>>();
            _downloadRequestValidator = new Mock<IValidator<Guid>>();
            _disabledocumentValidator = new Mock<IValidator<DisableDocumentRequest>>();
            _updateDocumentValidator = new Mock<IValidator<UpdateDocumentRequest>>();
            _updateDocumentFileValidator = new Mock<IValidator<UpdateDocumentFileRequest>>();
            _deleteDocumentValidator = new Mock<IValidator<DeleteDocumentRequest>>();
            _sut = new DocumentEngine(_logger.Object,
                _documentTypeResolver.Object,
                _configuration,
                _userRepository.Object,
                _mongoRepository.Object,
                _commentRepository.Object,
                _documentRepository.Object,
                _addDocumentRequestValidator.Object,
                _downloadRequestValidator.Object,
                _disabledocumentValidator.Object,
                _updateDocumentValidator.Object,
                _updateDocumentFileValidator.Object,
                _deleteDocumentValidator.Object);
            _userTestBuilder = new UserTestBuilder();
            _documentTestBuilder = new DocumentTestBuilder();
            _commentTestBuilder = new CommentTestBuilder();
            _createDocumentRequest = _documentTestBuilder.BuildCreate();
            _disableDocumentRequest = _documentTestBuilder.BuildDisable();
            _updateDocumentRequest = _documentTestBuilder.BuildUpdate();
            _updateDocumentFileRequest = _documentTestBuilder.BuildUpdateFile();
            _deleteDocumentRequest = _documentTestBuilder.BuildDelete();

            _documentTypeResolver.Setup(x => x(FileTypes.Document)).Returns(_mockDocumentDocument.Object);
            _documentTypeResolver.Setup(x => x(FileTypes.Image)).Returns(_mockDocumentImage.Object);
            _documentTypeResolver.Setup(x => x(FileTypes.Video)).Returns(_mockDocumentVideo.Object);
            _documentTypeResolver.Setup(x => x(FileTypes.Text)).Returns(_mockDocumentText.Object);

        }

        [Test]
        public void Given_ACreateDocumentRequest_When_ValidationFails_Then_AnExceptionShallBeCaptures()
        {
            var exception = new ValidationException("Test");
            _addDocumentRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.CreateDocument(_createDocumentRequest));

            ClassicAssert.That(exceptionReceived.Message.Equals(exception.Message));
            _documentRepository.Verify(x => x.InsertAsync(It.IsAny<Documents>()), Times.Never);
        }

        [Test]
        public void Given_AUpdateFileDocumentRequest_When_ValidationFails_Then_AnExceptionShallBeCaptures()
        {
            var exception = new ValidationException("Test");
            _updateDocumentFileValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.UpdateFileDocument(_updateDocumentFileRequest));

            ClassicAssert.That(exceptionReceived.Message.Equals(exception.Message));
            _documentRepository.Verify(x => x.InsertAsync(It.IsAny<Documents>()), Times.Never);
        }

        [Test]
        public void Given_AUpdateDocumentRequest_When_ValidationFails_Then_AnExceptionShallBeCaptures()
        {
            var exception = new ValidationException("Test");
            _updateDocumentValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.ModifyDocument(_updateDocumentRequest));

            ClassicAssert.That(exceptionReceived.Message.Equals(exception.Message));
            _documentRepository.Verify(x => x.InsertAsync(It.IsAny<Documents>()), Times.Never);
        }

        [Test]
        public void Given_ADisableDocumentRequest_When_ValidationFails_Then_AnExceptionShallBeCaptures()
        {
            var exception = new ValidationException("Test");
            _disabledocumentValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.DisableDocument(_disableDocumentRequest));

            ClassicAssert.That(exceptionReceived.Message.Equals(exception.Message));
            _documentRepository.Verify(x => x.InsertAsync(It.IsAny<Documents>()), Times.Never);
        }

        [Test]
        public void Given_ADeleteDocumentRequest_When_ValidationFails_Then_AnExceptionShallBeCaptures()
        {
            var exception = new ValidationException("Test");
            _deleteDocumentValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.DeleteDocument(_deleteDocumentRequest));

            ClassicAssert.That(exceptionReceived.Message.Equals(exception.Message));
            _documentRepository.Verify(x => x.InsertAsync(It.IsAny<Documents>()), Times.Never);
        }

        [Test]
        public void Given_ADownloadDocumentRequest_When_ValidationFails_Then_AnExceptionShallBeCaptures()
        {
            var exception = new ValidationException("Test");
            _downloadRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);
            var exceptionReceived = ClassicAssert.ThrowsAsync<ValidationException>(async () => await _sut.Download(Guid.NewGuid()));

            ClassicAssert.That(exceptionReceived.Message.Equals(exception.Message));
            _documentRepository.Verify(x => x.InsertAsync(It.IsAny<Documents>()), Times.Never);
        }

        [Test]
        public async Task Given_NewCreateDocumentTextRequest_When_AllValidationsPass_Then_DataAccessIsCalledToInsert_And_Validates_OneTransactionAdded()
        {
            var user = _userTestBuilder.DbBuild();
            List<Users> listUser = [user];

            _createDocumentRequest.File = null;
            _createDocumentRequest.Content = "test";
            Documents _document = new Documents()
            {
                Content = _createDocumentRequest.Content,
                CreateDate = DateTime.Now,
                CreateUser = _createDocumentRequest.CreateUser,
                Description = _createDocumentRequest.Description,
                DocumentType = new DocumentsType()
                {
                    Id = (int)FileTypes.Text,
                    Type = Enum.GetName(typeof(FileTypes), FileTypes.Text)
                },
                FileName = null,
                Hide = false,
                Id = Guid.NewGuid(),
                IdDocumentType = (int)FileTypes.Text,
                Users = user,
                IdUser = user.Id,
                ModifyDate = DateTime.Now,
                ModifyUser = _createDocumentRequest.CreateUser,
                Name = _createDocumentRequest.Name,
                SourceFile = null,
                TargetFile = null,
                Size = null
            };

            _addDocumentRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            _userRepository.Setup(x => x.GetAllByEmail(It.IsAny<string>())).ReturnsAsync(listUser);
            _mockDocumentText.Setup(x => x.CreateDocument(It.IsAny<CreateDocumentRequest>(), It.IsAny<Users>())).ReturnsAsync(_document);

            await _sut.CreateDocument(_createDocumentRequest);

            _documentRepository.Verify(x => x.InsertAsync(It.IsAny<Documents>()), Times.Once);
        }

        [Test]
        public async Task Given_NewCreateDocumentDocumentRequest_When_AllValidationsPass_Then_DataAccessIsCalledToInsert_And_Validates_OneTransactionAdded()
        {
            var user = _userTestBuilder.DbBuild();
            List<Users> listUser = [user];

            _createDocumentRequest.File = "test.txt";
            _createDocumentRequest.Content = null;
            Documents _document = new Documents()
            {
                Content = _createDocumentRequest.Content,
                CreateDate = DateTime.Now,
                CreateUser = _createDocumentRequest.CreateUser,
                Description = _createDocumentRequest.Description,
                DocumentType = new DocumentsType()
                {
                    Id = (int)FileTypes.Document,
                    Type = Enum.GetName(typeof(FileTypes), FileTypes.Document)
                },
                FileName = Path.GetFileName(_createDocumentRequest.File),
                Hide = false,
                Id = Guid.NewGuid(),
                IdDocumentType = (int)FileTypes.Document,
                Users = user,
                IdUser = user.Id,
                ModifyDate = DateTime.Now,
                ModifyUser = _createDocumentRequest.CreateUser,
                Name = _createDocumentRequest.Name,
                SourceFile = _createDocumentRequest.File,
                TargetFile = $"Document/{user.Id.ToString()}/{DateTime.Now.ToString("yyyyMMddhhmmss")}_{Path.GetFileName(_createDocumentRequest.File)}",
                Size = 10
            };

            _addDocumentRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            _userRepository.Setup(x => x.GetAllByEmail(It.IsAny<string>())).ReturnsAsync(listUser);
            _mockDocumentDocument.Setup(x => x.CreateDocument(It.IsAny<CreateDocumentRequest>(), It.IsAny<Users>())).ReturnsAsync(_document);


            await _sut.CreateDocument(_createDocumentRequest);

            _documentRepository.Verify(x => x.InsertAsync(It.IsAny<Documents>()), Times.Once);
        }

        [Test]
        public async Task Given_NewCreateDocumentImageRequest_When_AllValidationsPass_Then_DataAccessIsCalledToInsert_And_Validates_OneTransactionAdded()
        {
            var user = _userTestBuilder.DbBuild();
            List<Users> listUser = [user];

            _createDocumentRequest.File = "test.png";
            _createDocumentRequest.Content = null;
            Documents _document = new Documents()
            {
                Content = _createDocumentRequest.Content,
                CreateDate = DateTime.Now,
                CreateUser = _createDocumentRequest.CreateUser,
                Description = _createDocumentRequest.Description,
                DocumentType = new DocumentsType()
                {
                    Id = (int)FileTypes.Image,
                    Type = Enum.GetName(typeof(FileTypes), FileTypes.Image)
                },
                FileName = Path.GetFileName(_createDocumentRequest.File),
                Hide = false,
                Id = Guid.NewGuid(),
                IdDocumentType = (int)FileTypes.Image,
                Users = user,
                IdUser = user.Id,
                ModifyDate = DateTime.Now,
                ModifyUser = _createDocumentRequest.CreateUser,
                Name = _createDocumentRequest.Name,
                SourceFile = _createDocumentRequest.File,
                TargetFile = $"Image/{user.Id.ToString()}/{DateTime.Now.ToString("yyyyMMddhhmmss")}_{Path.GetFileName(_createDocumentRequest.File)}",
                Size = 10
            };

            _addDocumentRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            _userRepository.Setup(x => x.GetAllByEmail(It.IsAny<string>())).ReturnsAsync(listUser);
            _mockDocumentImage.Setup(x => x.CreateDocument(It.IsAny<CreateDocumentRequest>(), It.IsAny<Users>())).ReturnsAsync(_document);


            await _sut.CreateDocument(_createDocumentRequest);

            _documentRepository.Verify(x => x.InsertAsync(It.IsAny<Documents>()), Times.Once);
        }

        [Test]
        public async Task Given_NewCreateDocumentVideoRequest_When_AllValidationsPass_Then_DataAccessIsCalledToInsert_And_Validates_OneTransactionAdded()
        {
            var user = _userTestBuilder.DbBuild();
            List<Users> listUser = [user];

            _createDocumentRequest.File = "test.avi";
            _createDocumentRequest.Content = null;
            Documents _document = new Documents()
            {
                Content = _createDocumentRequest.Content,
                CreateDate = DateTime.Now,
                CreateUser = _createDocumentRequest.CreateUser,
                Description = _createDocumentRequest.Description,
                DocumentType = new DocumentsType()
                {
                    Id = (int)FileTypes.Video,
                    Type = Enum.GetName(typeof(FileTypes), FileTypes.Video)
                },
                FileName = Path.GetFileName(_createDocumentRequest.File),
                Hide = false,
                Id = Guid.NewGuid(),
                IdDocumentType = (int)FileTypes.Video,
                Users = user,
                IdUser = user.Id,
                ModifyDate = DateTime.Now,
                ModifyUser = _createDocumentRequest.CreateUser,
                Name = _createDocumentRequest.Name,
                SourceFile = _createDocumentRequest.File,
                TargetFile = $"Video/{user.Id.ToString()}/{DateTime.Now.ToString("yyyyMMddhhmmss")}_{Path.GetFileName(_createDocumentRequest.File)}",
                Size = 10
            };

            _addDocumentRequestValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            _userRepository.Setup(x => x.GetAllByEmail(It.IsAny<string>())).ReturnsAsync(listUser);
            _mockDocumentVideo.Setup(x => x.CreateDocument(It.IsAny<CreateDocumentRequest>(), It.IsAny<Users>())).ReturnsAsync(_document);


            await _sut.CreateDocument(_createDocumentRequest);

            _documentRepository.Verify(x => x.InsertAsync(It.IsAny<Documents>()), Times.Once);
        }

        [Test]
        public async Task Given_NewUpdateFileDocumentDocumentRequest_When_AllValidationsPass_Then_DataAccessIsCalledToUpdate_And_Validates_OneTransactionAdded()
        {
            var user = _userTestBuilder.DbBuild();
            List<Users> listUser = [user];
            var existDocument = _documentTestBuilder.DbBuild();

            _updateDocumentFileRequest.File = "test.txt";

            existDocument.ModifyUser = _updateDocumentFileRequest.ModifyUser;
            existDocument.ModifyDate = DateTime.Now;
            existDocument.FileName = Path.GetFileName(_updateDocumentFileRequest.File);
            existDocument.Name = _updateDocumentFileRequest.Name;
            existDocument.Description = _updateDocumentFileRequest.Description;
            existDocument.DocumentType.Id = (int)FileTypes.Document;
            existDocument.DocumentType.Type = Enum.GetName(typeof(FileTypes), FileTypes.Document);
            existDocument.SourceFile = _updateDocumentFileRequest.File;
            existDocument.TargetFile = $"Document/{user.Id.ToString()}/{DateTime.Now.ToString("yyyyMMddhhmmss")}_{Path.GetFileName(_updateDocumentFileRequest.File)}";
            existDocument.Size = 10;

            _updateDocumentFileValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            _userRepository.Setup(x => x.GetAllByEmail(It.IsAny<string>())).ReturnsAsync(listUser);
            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(existDocument);
            _mockDocumentDocument.Setup(x => x.UpdateDocument(It.IsAny<UpdateDocumentFileRequest>(), It.IsAny<Users>(), It.IsAny<Documents>())).ReturnsAsync(existDocument);


            await _sut.UpdateFileDocument(_updateDocumentFileRequest);

            _documentRepository.Verify(x => x.UpdateAsync(It.IsAny<Documents>()), Times.Once);
        }

        [Test]
        public async Task Given_NewUpdateFileDocumentImageRequest_When_AllValidationsPass_Then_DataAccessIsCalledToUpdate_And_Validates_OneTransactionAdded()
        {
            var user = _userTestBuilder.DbBuild();
            List<Users> listUser = [user];
            var existDocument = _documentTestBuilder.DbBuild();

            _updateDocumentFileRequest.File = "test.png";

            existDocument.ModifyUser = _updateDocumentFileRequest.ModifyUser;
            existDocument.ModifyDate = DateTime.Now;
            existDocument.FileName = Path.GetFileName(_updateDocumentFileRequest.File);
            existDocument.Name = _updateDocumentFileRequest.Name;
            existDocument.Description = _updateDocumentFileRequest.Description;
            existDocument.DocumentType.Id = (int)FileTypes.Image;
            existDocument.DocumentType.Type = Enum.GetName(typeof(FileTypes), FileTypes.Image);
            existDocument.SourceFile = _updateDocumentFileRequest.File;
            existDocument.TargetFile = $"Image/{user.Id.ToString()}/{DateTime.Now.ToString("yyyyMMddhhmmss")}_{Path.GetFileName(_updateDocumentFileRequest.File)}";
            existDocument.Size = 10;

            _updateDocumentFileValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            _userRepository.Setup(x => x.GetAllByEmail(It.IsAny<string>())).ReturnsAsync(listUser);
            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(existDocument);
            _mockDocumentImage.Setup(x => x.UpdateDocument(It.IsAny<UpdateDocumentFileRequest>(), It.IsAny<Users>(), It.IsAny<Documents>())).ReturnsAsync(existDocument);


            await _sut.UpdateFileDocument(_updateDocumentFileRequest);

            _documentRepository.Verify(x => x.UpdateAsync(It.IsAny<Documents>()), Times.Once);
        }

        [Test]
        public async Task Given_NewUpdateFileDocumentVideoRequest_When_AllValidationsPass_Then_DataAccessIsCalledToUpdate_And_Validates_OneTransactionAdded()
        {
            var user = _userTestBuilder.DbBuild();
            List<Users> listUser = [user];
            var existDocument = _documentTestBuilder.DbBuild();

            _updateDocumentFileRequest.File = "test.avi";

            existDocument.ModifyUser = _updateDocumentFileRequest.ModifyUser;
            existDocument.ModifyDate = DateTime.Now;
            existDocument.FileName = Path.GetFileName(_updateDocumentFileRequest.File);
            existDocument.Name = _updateDocumentFileRequest.Name;
            existDocument.Description = _updateDocumentFileRequest.Description;
            existDocument.DocumentType.Id = (int)FileTypes.Video;
            existDocument.DocumentType.Type = Enum.GetName(typeof(FileTypes), FileTypes.Video);
            existDocument.SourceFile = _updateDocumentFileRequest.File;
            existDocument.TargetFile = $"Video/{user.Id.ToString()}/{DateTime.Now.ToString("yyyyMMddhhmmss")}_{Path.GetFileName(_updateDocumentFileRequest.File)}";
            existDocument.Size = 10;

            _updateDocumentFileValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            _userRepository.Setup(x => x.GetAllByEmail(It.IsAny<string>())).ReturnsAsync(listUser);
            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(existDocument);
            _mockDocumentVideo.Setup(x => x.UpdateDocument(It.IsAny<UpdateDocumentFileRequest>(), It.IsAny<Users>(), It.IsAny<Documents>())).ReturnsAsync(existDocument);


            await _sut.UpdateFileDocument(_updateDocumentFileRequest);

            _documentRepository.Verify(x => x.UpdateAsync(It.IsAny<Documents>()), Times.Once);
        }

        [Test]
        public async Task Given_NewUpdateDocumentRequest_When_AllValidationsPass_Then_DataAccessIsCalledToUpdate_And_Validates_OneTransactionAdded()
        {
            var user = _userTestBuilder.DbBuild();
            List<Users> listUser = [user];
            var existDocument = _documentTestBuilder.DbBuild();

            _updateDocumentValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            _userRepository.Setup(x => x.GetAllByEmail(It.IsAny<string>())).ReturnsAsync(listUser);
            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(existDocument);


            await _sut.ModifyDocument(_updateDocumentRequest);

            _documentRepository.Verify(x => x.UpdateAsync(It.IsAny<Documents>()), Times.Once);
        }

        [Test]
        public async Task Given_NewUpdateDisabledRequest_When_AllValidationsPass_Then_DataAccessIsCalledToUpdate_And_Validates_OneTransactionAdded()
        {
            var user = _userTestBuilder.DbBuild();
            List<Users> listUser = [user];
            var existDocument = _documentTestBuilder.DbBuild();

            _disabledocumentValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            _userRepository.Setup(x => x.GetAllByEmail(It.IsAny<string>())).ReturnsAsync(listUser);
            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(existDocument);


            await _sut.DisableDocument(_disableDocumentRequest);

            _documentRepository.Verify(x => x.UpdateAsync(It.IsAny<Documents>()), Times.Once);
            _commentRepository.Verify(x => x.InsertAsync(It.IsAny<Comments>()), Times.Once);
        }

        [Test]
        public async Task Given_NewDeleteDocumentTextRequest_When_AllValidationsPass_Then_DataAccessIsCalledToDelete_And_Validates_OneTransactionAdded()
        {
           
            var document = _documentTestBuilder .DbBuild();
            document.Content = "test";
            document.SourceFile = null;
            document.TargetFile = null;
            document.FileName = null;
            document.IdDocumentType = (int)FileTypes.Text;
            document.DocumentType.Id = (int)FileTypes.Text;

            _deleteDocumentValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(document);
            _mockDocumentText.Setup(x => x.DeleteDocument(It.IsAny<DeleteDocumentRequest>(), It.IsAny<Documents>())).ReturnsAsync(document);

            await _sut.DeleteDocument(_deleteDocumentRequest);

            _documentRepository.Verify(x => x.DeleteAsync(It.IsAny<Documents>()), Times.Once);
        }

        [Test]
        public async Task Given_NewDeleteDocumentDocumentRequest_When_AllValidationsPass_Then_DataAccessIsCalledToDelete_And_Validates_OneTransactionAdded()
        {

            var document = _documentTestBuilder.DbBuild();
            document.Content = null;
            document.SourceFile = "test.txt";
            document.TargetFile = "test.txt";
            document.FileName = "test.txt";
            document.IdDocumentType = (int)FileTypes.Document;
            document.DocumentType.Id = (int)FileTypes.Document;

            _deleteDocumentValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(document);
            _mockDocumentDocument.Setup(x => x.DeleteDocument(It.IsAny<DeleteDocumentRequest>(), It.IsAny<Documents>())).ReturnsAsync(document);

            await _sut.DeleteDocument(_deleteDocumentRequest);

            _documentRepository.Verify(x => x.DeleteAsync(It.IsAny<Documents>()), Times.Once);
        }

        [Test]
        public async Task Given_NewDeleteDocumentImangeRequest_When_AllValidationsPass_Then_DataAccessIsCalledToDelete_And_Validates_OneTransactionAdded()
        {
            var document = _documentTestBuilder.DbBuild();
            document.Content = null;
            document.SourceFile = "test.png";
            document.TargetFile = "test.png";
            document.FileName = "test.png";
            document.IdDocumentType = (int)FileTypes.Image;
            document.DocumentType.Id = (int)FileTypes.Image;

            _deleteDocumentValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(document);
            _mockDocumentImage.Setup(x => x.DeleteDocument(It.IsAny<DeleteDocumentRequest>(), It.IsAny<Documents>())).ReturnsAsync(document);

            await _sut.DeleteDocument(_deleteDocumentRequest);

            _documentRepository.Verify(x => x.DeleteAsync(It.IsAny<Documents>()), Times.Once);
        }

        [Test]
        public async Task Given_NewDeleteDocumentVideoRequest_When_AllValidationsPass_Then_DataAccessIsCalledToDelete_And_Validates_OneTransactionAdded()
        {
            var document = _documentTestBuilder.DbBuild();
            document.Content = null;
            document.SourceFile = "test.avi";
            document.TargetFile = "test.avi";
            document.FileName = "test.avi";
            document.IdDocumentType = (int)FileTypes.Video;
            document.DocumentType.Id = (int)FileTypes.Video;

            _deleteDocumentValidator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Verifiable();
            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(document);
            _mockDocumentVideo.Setup(x => x.DeleteDocument(It.IsAny<DeleteDocumentRequest>(), It.IsAny<Documents>())).ReturnsAsync(document);

            await _sut.DeleteDocument(_deleteDocumentRequest);

            _documentRepository.Verify(x => x.DeleteAsync(It.IsAny<Documents>()), Times.Once);
        }

        [Test]
        public async Task Given_GetById_When_AllValidationsPass_Then_DataAccessIsCalledToInsert_And_Validates_ReturnedData()
        {
            var expectedDB = _documentTestBuilder.DbBuild();

            _documentRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(expectedDB);

            var response = await _sut.GetDocumentById(expectedDB.Id);

            ClassicAssert.That(response.Id == expectedDB.Id);
        }

        [TestCase(1, 10)]
        public async Task Given_APagination_WhenAllFieldsPass_Then_AListIsReturned(int pageNumber, int pageSize)
        {
            var pagination = new Pagination
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var expectedDBDocumentDatabaseModels = new List<Documents>(5);
            for (int index = 0; index < 5; index++)
            {
                var documentTestBuilder = new DocumentTestBuilder();
                var documentModel = documentTestBuilder.DbBuild();

                expectedDBDocumentDatabaseModels.Add(documentModel);
            }
            var expectedPaginatedList = new PaginatedList<Documents>()
            {
                Items = expectedDBDocumentDatabaseModels,
                Total = 5
            };

            _documentRepository.Setup(x => x.GetAllAsync(pagination)).ReturnsAsync(expectedPaginatedList);

            var actualPaginationResult = await _sut.GetAllDocument(pagination);

            ClassicAssert.IsNotNull(actualPaginationResult);
            ClassicAssert.IsTrue(actualPaginationResult.Items.Any());
            ClassicAssert.That(actualPaginationResult.Total > 0);
        }

        [Test]
        public async Task Given_GetBySearchText_When_AllValidationsPass_Then_DataAccessIsCalledToInsert_And_Validates_ReturnedData()
        {
            var expectedDB = _documentTestBuilder.DbBuild();
            var mongoDb = _documentTestBuilder.DbMongoBuild();
            List<Documents> listDocs = [expectedDB];
            List<MongoDocuments> listMongoDocs = new List<MongoDocuments>();

            _documentRepository.Setup(x => x.GetByCriteria(It.IsAny<Expression<Func<Documents, bool>>>())).ReturnsAsync(listDocs);
            _mongoRepository.Setup(x => x.SearchByText(It.IsAny<string>())).ReturnsAsync(listMongoDocs);

            var response = await _sut.GetDocumentByText("test");

            ClassicAssert.That(response.Count() > 0);
        }
    }
}

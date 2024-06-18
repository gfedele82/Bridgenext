using Bridgenext.DataAccess.Interfaces;
using Bridgenext.DataAccess.Repositories;
using Bridgenext.Engine.Interfaces;
using Bridgenext.Engine.Interfaces.Providers;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Enums;
using Bridgenext.Models.Schema.DB;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using UglyToad.PdfPig;

namespace Bridgenext.Engine.Strategy
{
    public class DocumentProcessDocument (ILogger<DocumentProcessImage> _logger,
        IMongoRepostory _mongoRepository,
        IMinioEngine _minioEngine,
        ICommentRepository _commentRepository) : IProcessDocumentByType
    {
        private readonly string path = "Document";

        public async Task<Documents> CreateDocument(CreateDocumentRequest addDocumentRequest, Users user)
        {
            _logger.LogInformation($"DocumentProcessDocument: Payload = {JsonConvert.SerializeObject(addDocumentRequest)}");

            Documents _document = new Documents()
            {
                Content = addDocumentRequest.Content,
                CreateDate = DateTime.Now,
                CreateUser = addDocumentRequest.CreateUser,
                Description = addDocumentRequest.Description,
                DocumentType = new DocumentsType()
                {
                    Id = (int)FileTypes.Document,
                    Type = Enum.GetName(typeof(FileTypes), FileTypes.Document)
                },
                FileName = Path.GetFileName(addDocumentRequest.File),
                Hide = false,
                Id = Guid.NewGuid(),
                IdDocumentType = (int)FileTypes.Document,
                Users = user,
                IdUser = user.Id,
                ModifyDate = DateTime.Now,
                ModifyUser = addDocumentRequest.CreateUser,
                Name = addDocumentRequest.Name,
                SourceFile = addDocumentRequest.File,
                TargetFile = $"{path}/{user.Id.ToString()}/{DateTime.Now.ToString("yyyyMMddhhmmss")}_{Path.GetFileName(addDocumentRequest.File)}"

            };

            try
            {

                _document = await _minioEngine.PutFile(_document);

                var process = await ProcessDocumentMongo(_document);

                if(!process)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateDocument: Payload = {JsonConvert.SerializeObject(addDocumentRequest)} - Error = {ex.Message}");

                return null;
            }

            return _document;
        }

        public async Task<Documents> UpdateDocument(UpdateDocumentFileRequest updateDocumnetFileRequest, Users user, Documents existDocument)
        {
            _logger.LogInformation($"UpdateDocument: Payload = {JsonConvert.SerializeObject(updateDocumnetFileRequest)}");

            await _minioEngine.DeleteFile(existDocument);

            await _mongoRepository.DeleteDocument(existDocument);

            existDocument.ModifyUser = updateDocumnetFileRequest.ModifyUser;
            existDocument.ModifyDate = DateTime.Now;
            existDocument.FileName = Path.GetFileName(updateDocumnetFileRequest.File);
            existDocument.Name = updateDocumnetFileRequest.Name;
            existDocument.Description = updateDocumnetFileRequest.Description;
            existDocument.DocumentType.Id = (int)FileTypes.Document;
            existDocument.DocumentType.Type = Enum.GetName(typeof(FileTypes), FileTypes.Document);
            existDocument.SourceFile = updateDocumnetFileRequest.File;
            existDocument.TargetFile = $"{path}/{user.Id.ToString()}/{DateTime.Now.ToString("yyyyMMddhhmmss")}_{Path.GetFileName(updateDocumnetFileRequest.File)}";

            try
            {
                existDocument = await _minioEngine.PutFile(existDocument);

                var process = await ProcessDocumentMongo(existDocument);

                if (!process)
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateDocument: Payload = {JsonConvert.SerializeObject(updateDocumnetFileRequest)} - Error = {ex.Message}");

                return null;
            }

            return existDocument;
        }

        public async Task<Documents> DeleteDocument(DeleteDocumentRequest deleteDocumnetFileRequest, Documents existDocument)
        {
            _logger.LogInformation($"DeleteDocument: Payload = {JsonConvert.SerializeObject(deleteDocumnetFileRequest)}");

            existDocument.ModifyDate = DateTime.Now;
            existDocument.ModifyUser = deleteDocumnetFileRequest.ModifyUser;

            var comments = await _commentRepository.GetByCriteria(x => x.IdDocumnet == existDocument.Id);

            foreach (var comment in comments)
            {
                await _commentRepository.DeleteAsync(comment);
            }

            await _minioEngine.DeleteFile(existDocument);

            string ext = Path.GetExtension(existDocument.SourceFile).ToUpper().Replace(".", "");

            if(ext.Equals("DOCX") || ext.Equals("PDF") || ext.Equals("TXT"))
            {
                await _mongoRepository.DeleteDocument(existDocument);
            }

            return existDocument;
        }

        private async Task<bool> ProcessDocumentMongo (Documents _document)
        {
            string ext = Path.GetExtension(_document.SourceFile).ToUpper().Replace(".", "");

            string fileContent = string.Empty;

            if (ext.Equals("DOCX"))
            {
                fileContent = ReadWordFile(_document.SourceFile);
            }
            else if (ext.Equals("PDF"))
            {
                fileContent = ReadPdfFile(_document.SourceFile);
            }
            else if (ext.Equals("TXT"))
            {
                fileContent = ReadTXTFile(_document.SourceFile);
            }

            if (!string.IsNullOrEmpty(fileContent))
            {
                var process = await _mongoRepository.CreateDocument(_document, fileContent);

                if (!process)
                {
                    await _minioEngine.DeleteFile(_document);

                    return false;
                }
            }

            return true;

        }

        private string ReadTXTFile(string path)
        {
            return File.ReadAllText(path);
        }

        private string ReadPdfFile(string path)
        {
            string response = string.Empty;

            using (PdfDocument document = PdfDocument.Open(path))
            {
                var text = new System.Text.StringBuilder();

                foreach (var page in document.GetPages())
                {
                    text.Append(page.Text);
                }

                response = text.ToString();
            }

            return response;
        }

        private string ReadWordFile(string path)
        {
            string response = string.Empty;
            StringBuilder sb = new StringBuilder();

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(path, false))
            {
                Body body = wordDoc.MainDocumentPart.Document.Body;
                foreach (var text in body.Descendants<Text>())
                {
                    sb.Append(text.Text);
                }

                response = sb.ToString();
            }

            return response;
        }

    }
}

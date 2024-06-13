using Bridgenext.Engine;
using Bridgenext.Engine.Interfaces;
using Bridgenext.Models.DTO.Request;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;

namespace Bridgenext.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController (ILogger<DocumentsController> _logger,
                                 IDocumentEngine _documentEngine) : ControllerBase
    {
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(CreateDocumentRequest), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateDocument(CreateDocumentRequest addDocumentRequest)
        {
            _logger.LogInformation($"CreateDocument POST API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(addDocumentRequest)}");

            try
            {
               var addCreateDocuument = await _documentEngine.CreateDocument(addDocumentRequest);

                return CreatedAtAction(nameof(GetDocument), new { id = addCreateDocuument.Id }, addCreateDocuument);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateDocument POST API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(addDocumentRequest)} error:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDocument(Guid id)
        {
            _logger.LogInformation($"GetDocument GET API called at {DateTime.Now} with id {id}");

            var existingDocument = await _documentEngine.GetDocumentById(id);
            if (existingDocument == null)
            {
                return NotFound();

            }

            return Ok(existingDocument);
        }

        [HttpGet("Download/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Download(Guid id)
        {
            _logger.LogInformation($"Download GET API called at {DateTime.Now} with id {id}");

            try
            {
                var download = await _documentEngine.Download(id);

                return new FileStreamResult(download.Item2, "application/octet-stream")
                {
                    FileDownloadName = download.Item1
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Download GET API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(id)} error:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Search/{text}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchByText(string text)
        {
            _logger.LogInformation($"SearchByText GET API called at {DateTime.Now} with text {text}");

            var documents = await _documentEngine.GetDocumentByText(text);

            if (documents == null)
                return NotFound();

            return Ok(documents);
        }

        [HttpPut("Disable/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DisableDocument(Guid id, DisableDocumentRequest disableDocumentRequest)
        {
            _logger.LogInformation($"DisableDocument PUT API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(disableDocumentRequest)}");

            try
            {
                if (id != disableDocumentRequest.Id)
                {
                    return BadRequest();
                }

                await _documentEngine.DisableDocument(disableDocumentRequest);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"DisableDocument POST API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(disableDocumentRequest)} error:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ModifyDocument(Guid id, UpdateDocumentRequest updateDocumentRequest)
        {
            _logger.LogInformation($"ModifyDocument PUT API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(updateDocumentRequest)}");

            try
            {
                if (id != updateDocumentRequest.Id)
                {
                    return BadRequest();
                }

                await _documentEngine.ModifyDocument(updateDocumentRequest);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"ModifyDocument POST API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(updateDocumentRequest)} error:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateFile/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ModifyDocumentFile(Guid id, UpdateDocumentFileRequest updateDocumentFileRequest)
        {
            _logger.LogInformation($"ModifyDocumentFile PUT API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(updateDocumentFileRequest)}");

            try
            {
                if (id != updateDocumentFileRequest.Id)
                {
                    return BadRequest();
                }

                await _documentEngine.UpdateFileDocument(updateDocumentFileRequest);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"ModifyDocumentFile POST API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(updateDocumentFileRequest)} error:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDocument(DeleteDocumentRequest deleteDocument)
        {
            _logger.LogInformation($"DeleteDocument DELETE API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(deleteDocument)}");

            try
            {

                var existDocument = await _documentEngine.DeleteDocument(deleteDocument);
                if (existDocument == null)
                {
                    return NotFound();
                }

                return Ok(existDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteDocument POST API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(deleteDocument.Id)} error:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }

    }
}

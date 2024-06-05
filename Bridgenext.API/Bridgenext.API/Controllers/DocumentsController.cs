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
                                 IDocumentEngine _documentEngine
                                 ) : ControllerBase
    {
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
       // [ProducesResponseType(typeof(CreateDocumentRequest), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateDocument(CreateDocumentRequest addDocumentRequest)
        {
            _logger.LogInformation($"CreateDocument POST API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(addDocumentRequest)}");

            try
            {
               var addCreateDocuument = await _documentEngine.CreateDocument(addDocumentRequest);

                //return CreatedAtAction(nameof(GetUser), new { id = addCreateDocuument.Id }, addCreateDocuument);
                return Ok(addCreateDocuument);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateDocument POST API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(addDocumentRequest)} error:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}

using Bridgenext.Engine;
using Bridgenext.Engine.Interfaces;
using Bridgenext.Models.DTO;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;

namespace Bridgenext.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController(ILogger<CommentsController> _logger,
                                 ICommentEngine _commentEngine) : ControllerBase
    {
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateComment(CreateCommetRequest addCommentRequest)
        {
            _logger.LogInformation($"CreateComment POST API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(addCommentRequest)}");

            try
            {
                var addCommentUser = await _commentEngine.CreateComment(addCommentRequest);

                return CreatedAtAction(nameof(GetComment), new { id = addCommentUser.Id }, addCommentUser);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateComment POST API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(addCommentRequest)} error:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetComment(Guid id)
        {
            _logger.LogInformation($"GetComment GET API called at {DateTime.Now} with id {id}");

            var existingUser = await _commentEngine.GetCommentById(id);
            if (existingUser == null)
            {
                return NotFound();

            }

            return Ok(existingUser);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllComments()
        {
            _logger.LogInformation($"GetAllComments GET API called at {DateTime.Now}");

            return Ok(await _commentEngine.GetAllComments());
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteComment(DeleteCommetRequest deleteComment)
        {
            _logger.LogInformation($"DeleteComment DELETE API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(deleteComment)}");

            try
            {

                var existComment = await _commentEngine.DeleteComment(deleteComment);
                if (existComment == null)
                {
                    return NotFound();
                }

                return Ok(existComment);
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteComment POST API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(deleteComment.Id)} error:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}

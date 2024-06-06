using Bridgenext.Engine.Interfaces;
using Bridgenext.Models.DTO;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Mime;

namespace Bridgenext.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(ILogger<UsersController> _logger,
                                 IUsersEngine _userEngine) : ControllerBase
    {

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateUser(CreateUserRequest addUserRequest)
        {
            _logger.LogInformation($"CreateUser POST API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(addUserRequest)}");

            try
            {
                var addCreateUser = await _userEngine.CreateUser(addUserRequest);

              //  return CreatedAtAction(nameof(GetUser), new { id = addCreateUser.Id }, addCreateUser);
                return Ok(addCreateUser);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateUser POST API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(addUserRequest)} error:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUser(Guid id)
        {
            _logger.LogInformation($"GetUser GET API called at {DateTime.Now} with id {id}");

            var existingUser = await _userEngine.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound();

            }

            return Ok(existingUser);
        }

        [HttpGet("like/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            _logger.LogInformation($"GetUserByEmail GET API called at {DateTime.Now} with id {email}");

            var existingUser = await _userEngine.GetUserByEmail(email);
            if (existingUser == null)
            {
                return NotFound();

            }

            return Ok(existingUser);
        }

        [HttpGet("exist/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserExist(string email)
        {
            _logger.LogInformation($"GetUserExist GET API called at {DateTime.Now} with email {email}");

            var existingUser = await _userEngine.GetUserExistByEmail(email);

            return Ok(existingUser);
        }
       

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserPaged([FromQuery] Pagination pagination)
        {
            _logger.LogInformation($"GetUserPaged GET API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(pagination)}");

            return Ok(await _userEngine.GetAllUsers(pagination));
        }

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ModifyUser(Guid id, UpdateUserRequest updateUser)
        {
            _logger.LogInformation($"ModifyUser PUT API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(updateUser)}");

            try
            {
                if (id != updateUser.Id)
                {
                    return BadRequest();
                }

                var existingCLUserLocation = await _userEngine.ModifyUser(updateUser);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"ModifyUser POST API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(updateUser)} error:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(DeleteUserRequest deleteUser)
        {
            _logger.LogInformation($"DeleteUser DELETE API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(deleteUser)}");

            try
            {

                var existingCLUserLocation = await _userEngine.DeleteUser(deleteUser);
                if (existingCLUserLocation == null)
                {
                    return NotFound();
                }

                return Ok(existingCLUserLocation);
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteUser POST API called at {DateTime.Now} with payload: {JsonConvert.SerializeObject(deleteUser.Id)} error:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}

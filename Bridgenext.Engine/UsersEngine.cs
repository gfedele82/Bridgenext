using Bridgenext.DataAccess.Interfaces;
using Bridgenext.DataAccess.DTOAdapter;
using Bridgenext.Engine.Interfaces;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.DTO.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FluentValidation;
using Bridgenext.Models.DTO;
using Bridgenext.Models.Constant.Exceptions;

namespace Bridgenext.Engine
{
    public class UsersEngine (ILogger<UsersEngine> _logger, 
        IUserRepository _userRepository,
        IAddressRepository _addressRepositoty,
        IValidator<CreateUserRequest> _addUserRequestValidator,
        IValidator<CreateAddressRequest> _addAddresesValidator,
        IValidator<DeleteUserRequest> _deleteUserRequestValidator,
        IValidator<UpdateUserRequest> _updateUserRequestValidator,
        IValidator<UpdateAddressRequest> _updateAddressRequestValidator,
        IValidator<string> _emailRequestValidator) : IUsersEngine
    {

        public async Task<UserDto> CreateUser(CreateUserRequest addUserRequest)
        {
            _logger.LogInformation($"CreateUser: Payload = {JsonConvert.SerializeObject(addUserRequest)}");

            await _addUserRequestValidator.ValidateAndThrowAsync(addUserRequest);

            foreach(var address in addUserRequest.Addresses)
            {
                await _addAddresesValidator.ValidateAndThrowAsync(address);
            }

            var dbUser = addUserRequest.ToDatabaseModel();

            dbUser = await _userRepository.InsertAsync(dbUser);

            return dbUser.ToDomainModel();
        }

        public async Task<UserDto> GetUserById(Guid id)
        {
            _logger.LogInformation($"GetUserById: Id = {id}");

            var dbUser = await _userRepository.GetAsync(id);

            return dbUser.ToDomainModel();

        }

        public async Task<bool> GetUserExistByEmail(string email)
        {
            _logger.LogInformation($"GetUserExistByEmail: email = {email}");

            await _emailRequestValidator.ValidateAndThrowAsync(email);

            var dbUser = await _userRepository.IdExistsAsync(email);

            return dbUser;

        }

        public async Task<List<UserDto>> GetUserByEmail(string email)
        {
            _logger.LogInformation($"GetUserByEmail: Email = {email}");

            var dbUser = await _userRepository.GetAllByEmail(email);

            return dbUser.ToDomainModel().ToList();

        }

        public async Task<GetPaginatedResponse<UserDto>> GetAllUsers(Pagination pagination)
        {
            _logger.LogInformation("GetAllUsers");

            var paginatedList = await _userRepository.GetAllAsync(pagination);

            return new GetPaginatedResponse<UserDto>
            {
                Total = paginatedList.Total,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                Items = paginatedList.Items.ToDomainModel()
            };
        }

        public async Task<UserDto> ModifyUser(UpdateUserRequest updateUser)
        {
            _logger.LogInformation($"ModifyCLUser: payload: {JsonConvert.SerializeObject(updateUser)}");

            await _updateUserRequestValidator.ValidateAndThrowAsync(updateUser);

            try
            {
                _userRepository.InitialTransaction();

                var existingUser = await _userRepository.GetAsync(updateUser.Id);

                existingUser = updateUser.ToDatabaseModel(existingUser);

                var updateAddress = updateUser.Addresses.FindAll(x => x.Id != Guid.Empty).ToList();

                foreach (var addrees in updateAddress)
                {
                    await _updateAddressRequestValidator.ValidateAndThrowAsync(addrees);
                }

                var newAddress = updateUser.Addresses.FindAll(x => x.Id == Guid.Empty).ToCreateModel();

                foreach (var addrees in newAddress)
                {
                    await _addAddresesValidator.ValidateAndThrowAsync(addrees);
                }

                foreach (var address in newAddress)
                {
                    var dbAddress = address.ToDatabaseModel(existingUser.Id);
                    await _addressRepositoty.InsertAsync(dbAddress);
                }           

                foreach (var address in updateAddress)
                {
                    var existAddress = await _addressRepositoty.GetAsync(address.Id);
                    var dbAddress = address.ToDatabaseModel(existAddress);
                    await _addressRepositoty.UpdateAsync(dbAddress);
                }

                existingUser = await _userRepository.UpdateAsync(existingUser);

                _userRepository.CommitTransaction();

                return existingUser.ToDomainModel();
            }
            catch (Exception ex)
            {
                _userRepository.FailTransaction();

                _logger.LogError($"ModifyUser Error: {ex.Message}");

                throw new ApplicationException(GeneralExceptions.SystemFail);

            }
        }

        public async Task<UserDto> DeleteUser(DeleteUserRequest deleteUser)
        {
            await _deleteUserRequestValidator.ValidateAndThrowAsync(deleteUser);

            _logger.LogInformation($"DeleteUser: Payload = {JsonConvert.SerializeObject(deleteUser)}");

            var existingUser = await _userRepository.GetAsync(deleteUser.Id);

            if (existingUser == null)
                return null;

            existingUser = deleteUser.ToDatabaseModel(existingUser);

            await _userRepository.DeleteAsync(existingUser);

            return existingUser.ToDomainModel();
        }
    }
}

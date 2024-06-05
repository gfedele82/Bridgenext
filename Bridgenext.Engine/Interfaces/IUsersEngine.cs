using Bridgenext.Models.DTO;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.DTO.Response;

namespace Bridgenext.Engine.Interfaces
{
    public interface IUsersEngine
    {

        Task<UserDto> CreateUser(CreateUserRequest addUserRequest);

        Task<UserDto> GetUserById(Guid id);

        Task<GetPaginatedResponse<UserDto>> GetAllUsers(Pagination pagination);

        Task<bool> GetUserExistByEmail(string email);

        Task<List<UserDto>> GetUserByEmail(string email);

        Task<UserDto> ModifyUser(UpdateUserRequest updateUser);

        Task<UserDto> DeleteUser(DeleteUserRequest deleteUser);
    }
}
